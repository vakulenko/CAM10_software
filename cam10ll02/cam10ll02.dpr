// --------------------------------------------------------------------------------
// ASCOM Camera driver low-level interaction library for cam10_v0.1
// Edit Log:
// Date		    	Who	 Vers	  Description
// -----------	---	-----	---------------------------------------------------------
// 17-feb-2015  VSS 0.1     Initial
// 25-mar-2015  VSS 0.2     Fixed bug with memory leak with hev event, enhanced frame reading procedure
// --------------------------------------------------------------------------------

library cam10ll02;

uses
  MyD2XX,
  Classes,
  SysUtils,
  Windows,
  SyncObjs;

{$R *.res}

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

//camera state consts
cameraIdle = 0;
cameraWaiting = 1;
cameraExposing = 2;
cameraReading = 3;
cameraDownload = 4;
cameraError = 5;

//GLobal variables}
var
adress:integer;
co: posl;
bufim:array[0..(CameraWidth)*CameraHeight-1] of byte;
bufim2:array[0..CameraWidth*CameraHeight-1] of word;
sdx,sy0,sdy:integer;
kolbyte:integer;
blevel:byte;
hev:TEvent;
bufa:integer;
zad:integer;
kbyte:integer;
//time to wait for frame
dlit1:integer;

//flag, camera connection state
isConnected : boolean = false;
//flag, image ready state
imageReady : boolean = false;
cameraState : integer = cameraIdle;

procedure writp;
begin
  Write_USB_Device_Buffer(FT_CAM10B,adress);
  adress:=0;
end;

//reading image from sensor
procedure posl.Execute;
var
x,y,buf:integer;
begin
  cameraState:=cameraReading;
  sleep(zad);
  bufa:=0;
  buf:=Read_USB_Device_Buffer(FT_CAM10A,@bufim,kolbyte);
  kbyte:=buf;
  if buf = kolbyte then
    for y:=0 to sdy-1 do
      for x:=0 to sdx-1 do
        bufim2[x+CameraWidth*(y+sy0)]:=bufim[x+y*(sdx+0)]
  else bufa:=1;
  Get_USB_Device_Status(FT_CAM10A);
  if FT_Q_Bytes > 0 then Purge_USB_Device(FT_CAM10A,FT_PURGE_RX);
  hev.SetEvent;
  cameraState:=cameraDownload;
  cameraState:=cameraIdle;
  imageReady:=true;
end;

procedure ComRead;
begin
  co:=posl.Create(true);
  co.FreeOnTerminate:=true;
  co.Priority:=tpNormal;
  co.Resume;
end;

procedure starti;
begin
   FT_Out_Buffer[adress+0]:=portfirst or $2;
   FT_Out_Buffer[adress+1]:=portfirst or $6;
   FT_Out_Buffer[adress+2]:=portfirst or $4;
   inc(adress,3);
end;

procedure bytei(val:byte);
var i:integer;
buf:byte;
begin
  for i:=0 to 7 do
  begin
    if (val and $80) <> 0 then buf:=portfirst
    else buf:=portfirst or $4;
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

procedure stopi;
begin
   FT_Out_Buffer[adress+0]:=portfirst or $4;
   FT_Out_Buffer[adress+1]:=portfirst or $6;
   FT_Out_Buffer[adress+2]:=portfirst or $2;
   FT_Out_Buffer[adress+3]:=portfirst;
   inc(adress,4);
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
end;

function readframe(x0,dx,y0,dy:integer):boolean;
begin
  writes($01,12+y0);
  writes($03,dy-1);
  writes($0b,$1);
  kolbyte:=dx*dy;
  sdx:=dx;
  sdy:=dy;
  sy0:=y0;
  ComRead;
  hev.WaitFor(dlit1);
  if bufa < 1 then result:=true
  else begin
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

//Set camera gain, return bool result
function cameraSetGain (val : integer) : WordBool;
begin
  writes($35,2*val);
  Result :=true;
end;

//Set camera offset, return bool result
function cameraSetOffset (val : smallint;aut:boolean) : WordBool;
begin
  writes($60,2*val);
  writes($61,2*val);
  writes($63,2*val+blevel);
  writes($64,2*val+blevel);
  if aut then writes($62,$0498)
  else writes($62,$049d);
  Result :=true;
end;

procedure resetchip;
begin
  adress:=0;
  for adress:=0 to 99 do
    FT_Out_Buffer[adress]:=portfirst;
  for adress:=100 to 199 do
    FT_Out_Buffer[adress]:=portfirst - $1;
  for adress:=200 to 299 do
    FT_Out_Buffer[adress]:=portfirst;
  writp;
end;

procedure byteo(val:byte);
var i:integer;
b,buf:byte;
begin
  b:=$fe;
  for i:=0 to 7 do
  begin
    if (b and $80) <> 0 then buf:=portfirst
    else buf:=portfirst+$4;
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
var ou:word;
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
  Result:=ou;
end;

function cameraConnect () : WordBool;  stdcall; export;
var  FT_OP_flag : boolean;
begin
  FT_OP_flag:=true;
  if (FT_OP_flag) then
    begin
      if Open_USB_Device_By_Serial_Number(FT_CAM10A,'CAM10A') <> FT_OK then FT_OP_flag := false;
    end;
  if (FT_OP_flag) then
    begin
      if Open_USB_Device_By_Serial_Number(FT_CAM10B,'CAM10B') <> FT_OK then FT_OP_flag := false;
    end;
  if (FT_OP_flag) then
    begin
      // BitMode
      if Set_USB_Device_BitMode(FT_CAM10B,$f7, $04)  <> FT_OK then FT_OP_flag := false;
    end;
  if (FT_OP_flag) then
    begin
      FT_Current_Baud:=100000;
      Set_USB_Device_BaudRate(FT_CAM10B);
      Set_USB_Device_LatencyTimer(FT_CAM10B,2);
      Set_USB_Device_LatencyTimer(FT_CAM10A,2);
      Set_USB_Device_TimeOuts(FT_CAM10A,250,100);
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
  isConnected := FT_OP_flag;
  cameraState := cameraIdle;
  if(FT_OP_flag=false) then cameraState := cameraError;
  Result := isConnected;
end;

//Disconnect camera, return bool result
function cameraDisconnect (): WordBool; stdcall; export;
var FT_OP_flag : boolean;
begin
  FT_OP_flag := true;
  if Close_USB_Device(FT_CAM10A) <> FT_OK then FT_OP_flag := false;
  if Close_USB_Device(FT_CAM10B) <> FT_OK then FT_OP_flag := false;
  if (FT_OP_flag=false) then cameraState := cameraError
  else cameraState := cameraIdle;
  IsConnected := not FT_OP_flag;
  Result:= FT_OP_flag;
end;

//return camera connection status
function cameraIsConnected () : WordBool; stdcall; export;
begin
  Result := isConnected;
end;

//starts exposure
function cameraStartExposure (startY,numY : integer; duration : double; gain,offset : integer; autoOffset : WordBool; sblevel : integer) : WordBool; stdcall; export;
var x:integer;
begin
  imageReady := false;
  cameraState:=cameraWaiting;
  cameraState:=cameraExposing;
  blevel:=sblevel;
  cameraSetGain(gain);
  cameraSetOffset(offset,autoOffset);
  dlit1:=round(duration*1000)+1000;
  x:=round(100*(duration*1000)/13);
  writes($09,x);
  zad:= round(duration*1000-40);
  if (zad < 0) then zad:=0;
  readframe(0,CameraWidth,startY,numY);
  Result := true;
end;

//Get camera state
//0 CameraIdle At idle state, available to start exposure
//1 CameraWaiting Exposure started but waiting (for shutter, trigger, filter wheel, etc.)
//2 CameraExposing Exposure currently in progress
//3 CameraReading CCD array is being read out (digitized)
//4 CameraDownload Downloading data to PC
//5 CameraError Camera error condition serious enough to prevent further operations (connection fail, etc.).
function cameraGetCameraState : integer; stdcall; export;
begin
  Result := cameraState;
end;

//Check ImageReady flag
function cameraGetImageReady : WordBool; stdcall; export;
begin
  Result := imageReady;
end;

//Get back pointer to ASCOM driver
function cameraGetImage : dword; stdcall; export;
begin
  Result := dword(@bufim2);
end;

exports cameraConnect;
exports cameraDisconnect;
exports cameraIsConnected;
exports cameraStartExposure;
exports cameraGetCameraState;
exports cameraGetImageReady;
exports cameraGetImage;

begin
  hev := TEvent.Create(nil, false, false, '');
end.

