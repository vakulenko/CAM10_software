unit cam10;

interface

uses MyD2XX, Classes, SysUtils, Windows, SyncObjs;

type
  posl = class(TThread)
  private
    { Private declarations }
  protected
    procedure Execute; override;
  end;

const
portfirst  = $f9;
CameraWidth = 1280;
CameraHeight = 1024;
razm = 50;

var
adress:integer;
co: posl;
bufim:array[0..(CameraWidth)*CameraHeight-1] of byte;//array[0..(CameraWidth+1)*CameraHeight-1] of byte;
bufim2:array[0..CameraWidth*CameraHeight-1] of integer;
sx0,sdx,sy0,sdy:integer;
kolbyte:integer;
blevel:byte;
//hev:THandle;
hev:TEvent;
bufa:integer;
zad:integer;
kbyte:integer;
dlit1:integer; //сколько ждать кадр

IsConnected                         : boolean = false;
//mImageReady                         : boolean = false;

function QueueStatus:integer;
function CameraConnect () : WordBool;
function CameraDisConnect (): WordBool;
function CameraSetOffset (val : smallint;aut:boolean) : WordBool;
function CameraSetGain (val : integer) : WordBool;
function readframe(x0,dx,y0,dy:integer):boolean;
function reads(adr:byte): word;
procedure writes(adr:byte;val:word);

implementation

procedure writp;
begin
Write_USB_Device_Buffer(FT_CAM10B,adress);
adress:=0;
end;

procedure posl.Execute;
var
x,y,buf:integer;
begin
 sleep(zad);
 bufa:=0;
 buf:=Read_USB_Device_Buffer(FT_CAM10A,@bufim,kolbyte);kbyte:=buf;
 if buf = kolbyte then
 for y:=0 to sdy-1 do for x:=0 to sdx-1 do bufim2[x+CameraWidth*(y+sy0)]:=bufim[x+y*(sdx+0)]
 else bufa:=1;
 Get_USB_Device_Status(FT_CAM10A);
 if FT_Q_Bytes > 0 then
 Purge_USB_Device(FT_CAM10A,FT_PURGE_RX);
 hev.SetEvent;
end;

procedure ComRead;
begin
  co:=posl.Create(true);
  co.FreeOnTerminate:=true;
  co.Priority:=tpNormal;//Lower;//st;//r;//Normal;
  co.Resume;
end;

function readframe(x0,dx,y0,dy:integer):boolean;
begin
writes($01,12+y0);
writes($03,dy-1);
writes($0b,$1);
kolbyte:=dx*dy;         //(dx+1)*dy;
sdx:=dx;sdy:=dy;sy0:=y0;
ComRead;
hev.WaitFor(dlit1);
if bufa < 1 then result:=true else
begin
  Purge_USB_Device(FT_CAM10A, FT_PURGE_RX or FT_PURGE_TX);
  Purge_USB_Device(FT_CAM10B, FT_PURGE_RX or FT_PURGE_TX);
 result:=false;
end;
end;

function QueueStatus:integer;
begin
 Get_USB_Device_QueueStatus(FT_CAM10A);
 result:=FT_Q_Bytes;
end;

{Set camera gain, return bool result}
function CameraSetGain (val : integer) : WordBool; //stdcall; export;
begin
 writes($35,2*val);
 Result :=true;
end;

{Set camera offset, return bool result}
function CameraSetOffset (val : smallint;aut:boolean) : WordBool;// stdcall; export;
begin
 writes($60,2*val);
 writes($61,2*val);
 writes($63,2*val+blevel);
 writes($64,2*val+blevel);
 if aut then writes($62,$0498) else writes($62,$049d);
 Result :=true;
end;

procedure resetchip;
begin
 adress:=0;
 for adress:=0 to 99 do FT_Out_Buffer[adress]:=portfirst;
 for adress:=100 to 199 do FT_Out_Buffer[adress]:=portfirst - $1 ;//and $fe;
 for adress:=200 to 299 do FT_Out_Buffer[adress]:=portfirst;
 writp;
end;

procedure starti;
begin
   FT_Out_Buffer[adress+0]:=portfirst or $2;
   FT_Out_Buffer[adress+1]:=portfirst or $6;
   FT_Out_Buffer[adress+2]:=portfirst or $4;
   inc(adress,3);
end;

procedure stopi;
begin
   FT_Out_Buffer[adress+0]:=portfirst or $4;
   FT_Out_Buffer[adress+1]:=portfirst or $6;
   FT_Out_Buffer[adress+2]:=portfirst or $2;
   FT_Out_Buffer[adress+3]:=portfirst;
   inc(adress,4);
end;

procedure bytei(val:byte);
var
i:integer;
buf:byte;
begin
 for i:=0 to 7 do
    begin
     if (val and $80) <> 0 then buf:=portfirst else buf:=portfirst or $4;
     val:=val shl 1;
     FT_Out_Buffer[adress+0]:=buf;
     FT_Out_Buffer[adress+1]:=buf or $2;
     FT_Out_Buffer[adress+2]:=buf;
     inc(adress,3);
    end;
    FT_Out_Buffer[adress+0]:=portfirst;
    FT_Out_Buffer[adress+1]:=portfirst or $2;
    FT_Out_Buffer[adress+2]:=portfirst or $4;
    inc(adress,3);
end;

procedure byteo(val:byte);
var i:integer;
b,buf:byte;
begin
 b:=$fe;
 for i:=0 to 7 do
    begin
     if (b and $80) <> 0 then buf:=portfirst else buf:=portfirst+$4;
     b:=2*b;
     FT_Out_Buffer[adress+0]:=portfirst;
     FT_Out_Buffer[adress+1]:=portfirst or $2;
     FT_Out_Buffer[adress+2]:=buf;
     inc(adress,3);
    end;
    FT_Out_Buffer[adress+0]:=portfirst or val;
    FT_Out_Buffer[adress+1]:=portfirst or (val or $2);
    FT_Out_Buffer[adress+2]:=portfirst or val;
    inc(adress,3);
end;

function reads(adr:byte): word;
var
i:integer;
ou:word;
bufi:array[0..2047] of byte;
begin

 Purge_USB_Device(FT_CAM10B, FT_PURGE_TX);

 adress:=0;
 starti;
 bytei($ba);
 bytei(adr);
 starti;
 bytei($bb);
 byteo($4);
 byteo($0);
 stopi;
 writp;
 sleep(100);

 Get_USB_Device_Status(FT_CAM10B);
 Read_USB_Device_Buffer(FT_CAM10B,@bufi,FT_Q_Bytes);
 ou:=0;
 if (bufi[$58] and $08) <> 0 then ou:=ou+$8000;
 if (bufi[$5b] and $08) <> 0 then ou:=ou+$4000;
 if (bufi[$5e] and $08) <> 0 then ou:=ou+$2000;
 if (bufi[$61] and $08) <> 0 then ou:=ou+$1000;
 if (bufi[$64] and $08) <> 0 then ou:=ou+$0800;
 if (bufi[$67] and $08) <> 0 then ou:=ou+$0400;
 if (bufi[$6a] and $08) <> 0 then ou:=ou+$0200;
 if (bufi[$6d] and $08) <> 0 then ou:=ou+$0100;
 if (bufi[$73] and $08) <> 0 then ou:=ou+$80;
 if (bufi[$76] and $08) <> 0 then ou:=ou+$40;
 if (bufi[$79] and $08) <> 0 then ou:=ou+$20;
 if (bufi[$7c] and $08) <> 0 then ou:=ou+$10;
 if (bufi[$7f] and $08) <> 0 then ou:=ou+$08;
 if (bufi[$82] and $08) <> 0 then ou:=ou+$04;
 if (bufi[$85] and $08) <> 0 then ou:=ou+$02;
 if (bufi[$88] and $08) <> 0 then ou:=ou+$01;
// for i:=0 to FT_Q_Bytes-1 do if (bufi[i] and $08) <> 0 then bufi[i]:=$ff else bufi[i]:=0;
 Result:=ou;
end;

procedure writes(adr:byte;val:word);
begin
 adress:=0;
 starti;
 bytei($ba);
 bytei(adr);
 bytei(hi(val));
 bytei(lo(val));
 stopi;
 writp;
 //sleep(10);
end;

function CameraConnect () : WordBool;  //stdcall; export;
var  FT_flag, FT_OP_flag : boolean;
I : Integer;
begin
 FT_flag:=false;
 GetFTDeviceCount;
 I := FT_Device_Count-1;
 while I >= 0 do
  begin
   GetFTDeviceSerialNo(I);
   if pos('CAM10',FT_Device_String) <> 0 then FT_flag:=true;    //???? ????????? cam81 - ??????????
   GetFTDeviceDescription(I);
   Dec(I);
  end;
  FT_OP_flag:=true;
  if FT_flag then
   begin
    if Open_USB_Device_By_Serial_Number(FT_CAM10A,'CAM10A') <> FT_OK then FT_OP_flag := false;
    if Open_USB_Device_By_Serial_Number(FT_CAM10B,'CAM10B') <> FT_OK then FT_OP_flag := false;
    if Set_USB_Device_BitMode(FT_CAM10B,$f7, $04)  <> FT_OK then FT_OP_flag := false;             // BitMode
    FT_Current_Baud:=100000;
    Set_USB_Device_BaudRate(FT_CAM10B);
    FT_OP_flag := true;

    Set_USB_Device_LatencyTimer(FT_CAM10B,2);       //???????????? ??????????????
    Set_USB_Device_LatencyTimer(FT_CAM10A,2);
    Set_USB_Device_TimeOuts(FT_CAM10A,250,100);
    hev := TEvent.Create(nil, false, false, '');

    resetchip;
    writes($1e,$8100);
    writes($20,$0104);

    CameraSetGain (63);
    
    writes($60,0);
    writes($61,0);
    writes($63,blevel);
    writes($64,blevel);
    writes($62,$049d);

   end;
 IsConnected := FT_flag and FT_OP_flag;
 Result := FT_flag and FT_OP_flag;
end;

{Disconnect camera, return bool result}
function CameraDisConnect (): WordBool;// stdcall; export;
var FT_OP_flag : boolean;
begin
 FT_OP_flag := true;
 if Close_USB_Device(FT_CAM10A) <> FT_OK then FT_OP_flag := false;   //???????? ?????????
 if Close_USB_Device(FT_CAM10B) <> FT_OK then FT_OP_flag := false;
 IsConnected := not FT_OP_flag;
 Result:= FT_OP_flag;
end;

end.