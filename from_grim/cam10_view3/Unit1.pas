unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ExtCtrls, MyD2XX, ComCtrls, Buttons, IniFiles, Spin, Math, Cam10, AVIFile32;

type
  TForm1 = class(TForm)
    Memo1: TMemo;
    Button4: TButton;
    Timer1: TTimer;
    Image1: TImage;
    TrackBar1: TTrackBar;
    TrackBar2: TTrackBar;
    Label1: TLabel;
    Label2: TLabel;
    SpeedButton1: TSpeedButton;
    SpeedButton2: TSpeedButton;
    Image2: TImage;
    Button1: TButton;
    SpinEdit1: TSpinEdit;
    Edit1: TEdit;
    Button2: TButton;
    Button3: TButton;
    Edit2: TEdit;
    Label3: TLabel;
    Label4: TLabel;
    CheckBox2: TCheckBox;
    Image3: TImage;
    Label5: TLabel;
    CheckBox1: TCheckBox;
    RadioGroup1: TRadioGroup;
    SpeedButton3: TSpeedButton;
    CheckBox3: TCheckBox;
    CheckBox4: TCheckBox;
    Edit3: TEdit;
    SpeedButton4: TSpeedButton;
    SpeedButton5: TSpeedButton;
    procedure Memo1Change(Sender: TObject);
    procedure Timer1Timer(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure FormClose(Sender: TObject);
    procedure SpeedButton1Click(Sender: TObject);
    procedure TrackBar1Change(Sender: TObject);
    procedure TrackBar2Change(Sender: TObject);
    procedure FormMD(Sender: TObject; Button:                         //?????????? ??????? ??????????? ??? ??????????? ? "????"
TMouseButton; Shift: TShiftState; X, Y: Integer);
    procedure Button4Click(Sender: TObject);
    procedure Button1Click(Sender: TObject);
    procedure Button2Click(Sender: TObject);
    procedure Button3Click(Sender: TObject);
    procedure CheckBox2Click(Sender: TObject);
    procedure SpeedButton2Click(Sender: TObject);
    procedure SpinEdit1Change(Sender: TObject);
    procedure SpeedButton3Click(Sender: TObject);
    procedure SpeedButton5Click(Sender: TObject);
    procedure SpeedButton4Click(Sender: TObject);
  private
    { Private declarations }
    TekFile      : PAVIFile;
    TekAVIStream : PAVIStream;
    FramePos : Integer;
  public
    { Public declarations }
  end;

const
HalfWidth = CameraWidth div 2;
HalfHeight = CameraHeight div 2;

var
  Form1: TForm1;
  InImage,InImage3:TBitmap;
  bufim3:array[0..CameraWidth*CameraHeight-1] of integer;
  gis:array[0..255] of dword;
  StartXb,StartYb,SXb,SYb,SY0:integer;
  ConfigFile:TIniFile;
  evyd:boolean;
  dx,y0,dy:integer;

implementation

{$R *.dfm}

procedure ris(iso:integer);
var
x,y:integer;
line : pByteArray;
bline:integer;
miso,diso:integer;
begin
fillchar(gis,sizeof(gis),0);
for y:=0 to CameraHeight-1 do
  begin
  Line :=InImage.ScanLine[y];
  for x:=0 to CameraWidth-1 do
    begin
     bline:=bufim3[x+y*CameraWidth];
     bline:=bline shl iso;
     if bline > 255 then bline:=255;
     Line^[3*x+0]:=bline;
     Line^[3*x+1]:=bline;
     Line^[3*x+2]:=bline;
     inc(gis[bline]);
    end;
  end;

  for y:=0 to razm-1 do
  begin
  Line :=InImage3.ScanLine[y];
  for x:=0 to razm-1 do
    begin
     bline:=bufim3[x+SXb+(y+SYb)*CameraWidth];
     bline:=bline shl iso;
     if bline > 255 then bline:=255;
     Line^[3*x+0]:=bline;
     Line^[3*x+1]:=bline;
     Line^[3*x+2]:=bline;
    end;
  end;

  Form1.Image1.Picture.Bitmap:=InImage;
  Form1.Image3.Picture.Bitmap:=InImage3;

  diso:= round(Power(2,iso));
  miso:=256 div diso;

  Form1.Image2.Canvas.FillRect(Form1.Image2.Canvas.ClipRect);
  Form1.Image2.Canvas.Pen.Color:=$f0f0f0;Form1.Image2.Canvas.MoveTo(0,Form1.Image2.Height-1);
  for x:=0 to miso-1 do
   begin
    y:=round(8*ln(gis[diso*x]+1));
    Form1.Image2.Canvas.LineTo(diso*x,Form1.Image2.Height-1-y);
   end; 
end;

procedure writeg;
begin
 if Form1.SpeedButton5.down then
  begin
   Form1.Edit3.text:='fr'+inttostr(Form1.FramePos);
                         AVIStreamWrite(Form1.tekAVIStream,
                         Form1.FramePos,
                         1,
                         InImage.ScanLine[InImage.Height-1],    // ?????? ?????? ? ????????
                         InImage.Width * InImage.Height * 3,  // ??????
                         0,
                         nil,
                         nil);
   Inc(Form1.FramePos);
  end;
end;

procedure bining;
var
x,y:integer;
buf:integer;
f:file;
dan:integer;
begin
if Form1.CheckBox4.Checked then
for y:=0 to HalfHeight-1 do
 for x:=0 to HalfWidth-1 do
  begin
   buf:= bufim2[2*x+2*y*1280]+bufim2[2*x+1+2*y*1280]+bufim2[2*x+2*y*1280+1280]+bufim2[2*x+1+2*y*1280+1280];
   bufim3[2*x+2*y*1280]:=buf;
   bufim3[2*x+1+2*y*1280]:=buf;
   bufim3[2*x+2*y*1280+1280]:=buf;
   bufim3[2*x+1+2*y*1280+1280]:=buf;
  end else
  for x:=0 to CameraWidth*CameraHeight-1 do bufim3[x]:=bufim2[x];
  if Form1.CheckBox1.Checked then
   begin
    assignfile(f,'bias.bin');
    {$I-}
    reset(f,4);
    {$I+}
    if ioresult = 0 then
    begin
     for x:=0 to CameraWidth*CameraHeight-1 do
       begin
        blockread(f,dan,1);
        bufim3[x]:=bufim3[x]-dan+10;
        if bufim3[x] < 0 then bufim3[x]:=0;
       end;
     closefile(f);
    end;
   end;
end;


procedure TForm1.Memo1Change(Sender: TObject);
begin
 Memo1.Clear;
end;

procedure TForm1.Timer1Timer(Sender: TObject);
begin
 if SpeedButton2.Down then
  begin
   Button4.Click;
  end;
end;

procedure TForm1.FormMD(Sender: TObject; Button:                         //?????????? ??????? ??????????? ??? ??????????? ? "????"
TMouseButton; Shift: TShiftState; X, Y: Integer);
const
ykon = CameraHeight - razm;
xkon = CameraWidth - razm;
begin
  SXb := round(X*(CameraWidth/Image1.Width)-razm div 2);
  if SXb < 0 then SXb:=0;
  if SXb > xkon then SXb:=xkon;
  SYb := round(Y*(CameraHeight/Image1.Height)-razm div 2);
  if SYb < 0 then SYb:=0;
  if SYb > ykon then SYb:=ykon;
  SY0 :=round(Y*(CameraHeight/Image1.Height)-256);
  if SY0 < 0 then SY0:=0;
  if SY0 > 512 then SY0:=512;
 //Memo1.Lines.Add(inttostr(SXb)+' '+inttostr(SYb));
 ris(RadioGroup1.ItemIndex);
end;

procedure TForm1.FormCreate(Sender: TObject);
begin
   InImage:=TBitmap.Create;
   InImage.PixelFormat:=pf24bit;
   InImage.Height:=CameraHeight;
   InImage.Width:=CameraWidth;

   InImage3:=TBitmap.Create;
   InImage3.PixelFormat:=pf24bit;
   InImage3.Height:=razm;
   InImage3.Width:=razm;

   Image1.Canvas.Brush.Color := clSilver;
   Image1.Canvas.FillRect(Image1.Canvas.ClipRect);
   Image2.Canvas.Brush.Color := clBlack;
   Image2.Canvas.FillRect(Image2.Canvas.ClipRect);
   Image3.Canvas.Brush.Color := clSilver;
   Image3.Canvas.FillRect(Image3.Canvas.ClipRect);

   SXb:=(CameraWidth-razm) div 2;
   SYb:=(CameraHeight-razm) div 2;
   SY0:= 256;

   ConfigFile:=TIniFile.Create(GetCurrentDir+'\cam10.ini');
   if not ConfigFile.SectionExists('TIMES') then
    begin
     ConfigFile.WriteInteger('TIMES','T1',240);
    end else
    begin
     SpinEdit1.Value:=ConfigFile.ReadInteger('TIMES','T1',240);
    end;

   if not ConfigFile.SectionExists('BLEVELS') then
    begin
     ConfigFile.WriteInteger('BLEVELS','B1',17);
     blevel:=17;
    end else
    begin
     blevel:=ConfigFile.ReadInteger('BLEVELS','B1',17);
    end;
end;

procedure TForm1.FormClose(Sender: TObject);
begin
   ConfigFile.WriteInteger('TIMES','T1',SpinEdit1.Value);
   InImage.Destroy;
end;

procedure TForm1.SpeedButton1Click(Sender: TObject);                      //???????? - ????????? ?????????
begin
  if SpeedButton1.Down then
  begin
  if CameraConnect then
   begin
    TrackBar1.Enabled:=true;
    Trackbar2.Enabled:=true;
    Button4.Enabled:=true;
    SpeedButton2.Enabled:=true;
    CheckBox2.Enabled:=true;
    evyd:=false;
    Memo1.Lines.Add('connect')
   end else SpeedButton1.Down:=false;
  end else
   begin
    if CameraDisConnect then
    begin
     Timer1.Enabled:=false;
     TrackBar1.Enabled:=false;
     Trackbar2.Enabled:=false;
     Button4.Enabled:=false;
     SpeedButton2.Down:=false;
     SpeedButton2.Enabled:=false;
     CheckBox2.Enabled:=false;
     Memo1.Lines.Add('disconnect')
    end;
   end;
end;

procedure TForm1.TrackBar1Change(Sender: TObject);
begin
 CameraSetGain(TrackBar1.Position);
end;

procedure TForm1.TrackBar2Change(Sender: TObject);
begin
 CameraSetOffset(TrackBar2.Position,CheckBox2.Checked);
end;

procedure TForm1.Button4Click(Sender: TObject);
var
x:integer;
begin
 if not evyd then
 begin
  x:=round(100*SpinEdit1.Value/13);
  //Memo1.Lines.Add(inttostr(x));
  writes($09,x);
  evyd:=true;
  zad:= SpinEdit1.Value-40;
  if zad < 0 then zad:=0;
 end;
 dx:=1280;
 if CheckBox3.Checked then
  begin
   dy:=512;
   y0:=SY0
  end
                      else
  begin
   dy:=1024;
   y0:=0
  end;
 //writes($04,dx-1);
 if readframe(0,dx,y0,dy) then
  begin
   bining;
   ris(RadioGroup1.ItemIndex);
   writeg;
  end;
 if bufa <> 0 then
  begin Memo1.Lines.Add(inttostr(bufa));
 Memo1.Lines.Add(inttostr(kbyte)+' '+inttostr(kolbyte-kbyte));
 end;
end;

procedure TForm1.Button1Click(Sender: TObject);
var
f:file;
begin
 assignfile(f,Edit3.text+'.raw');
 rewrite(f,1);
 blockwrite(f,bufim,sizeof(bufim));
 closefile(f);  
end;

procedure TForm1.Button2Click(Sender: TObject);
var
val:integer;
begin
 if not TryStrToInt(Edit1.Text,val) then Memo1.Lines.Add('error') else
 begin
  Edit2.Text:= '$'+inttohex(reads(val),4);
 end;
end;

procedure TForm1.Button3Click(Sender: TObject);
var
val,adr:integer;
begin
 if not TryStrToInt(Edit1.Text,adr) then Memo1.Lines.Add('error') else
 begin
  if not TryStrToInt(Edit2.Text,val) then Memo1.Lines.Add('error') else
  writes(adr,val);
 end;
end;

procedure TForm1.CheckBox2Click(Sender: TObject);
begin
 CameraSetOffset(TrackBar2.Position,CheckBox2.Checked);
end;

procedure TForm1.SpeedButton2Click(Sender: TObject);
begin
 if SpeedButton2.Down then timer1.Enabled:=true else timer1.Enabled:=false;
 //Button4.Click;
end;

procedure TForm1.SpinEdit1Change(Sender: TObject);
begin
SpinEdit1.Increment:=1;
if SpinEdit1.Value >= 10 then SpinEdit1.Increment:=2;
if SpinEdit1.Value >= 50 then SpinEdit1.Increment:=10;
if SpinEdit1.Value >= 100 then SpinEdit1.Increment:=20;
if SpinEdit1.Value >= 500 then SpinEdit1.Increment:=100;
evyd:=false;
end;

procedure TForm1.SpeedButton3Click(Sender: TObject);
begin
 if SpeedButton3.Down then writes($20,$8104) else writes($20,$0104);
end;


procedure TForm1.SpeedButton5Click(Sender: TObject);
var
 bmiHeader     : TBitmapInfoHeader;      // BMP ?????????
 AVIStreamInfo : TAVIStreamInfo;
begin
 if SpeedButton5.Down then
   begin
      AVIFileInit;
      AVIFileOpen(tekFile, PChar(Edit3.text+'.avi'), OF_CREATE or OF_WRITE, nil);

      FillChar(bmiHeader,SizeOf(TBitmapInfoHeader),0);
      with bmiHeader do // BITMAPINFOHEADER
       begin
         biSize       := SizeOf(TBitmapInfoHeader);
         biWidth:= InImage.Width;
         biHeight:= InImage.Height;
         biPlanes     := 1;
         biBitCount   := 24;                   // bits per pix.
         biCompression:= BI_RGB;
         biSizeImage  := (((biWidth * biBitCount) + 31) div 32) * 4 * biHeight;
       end;

       FillChar(AVIStreamInfo,SizeOf(AVIStreamInfo),0);
       with AVIStreamInfo do
        begin
         dwSuggestedBufferSize:=bmiHeader.biSizeImage;
         fccType     := streamtypeVIDEO;
         dwFlags     := 0;
         dwScale     := 1;                      // ?????? ? ??????? = dwRate / dwScale
         dwRate      := 10;                      //fps
         dwStart     := 0;
         dwLength    := 1;
         dwSuggestedBufferSize:=bmiHeader.biSizeImage;
         rectFrame.Right  := bmiHeader.biWidth;
         rectFrame.Bottom := bmiHeader.biHeight;
        end;

       AVIFileCreateStream(TekFile,TekAVIStream,AVIStreamInfo);
       AVIStreamSetFormat(tekAVIStream,0,@bmiHeader,SizeOf(TBitmapInfoHeader));
       FramePos := 0;
       CheckBox1.Enabled:=false;
   end else
      begin
        AVIStreamRelease(tekAVIStream);
        AVIFileRelease(TekFile);
        AVIFileExit;
        Edit3.Text:='Noname';
        CheckBox1.Enabled:=true;
        Memo1.Lines.Add(inttostr(framepos+1)+' кадров записано');
      end;
  end;

procedure TForm1.SpeedButton4Click(Sender: TObject);
var
f:file;
begin
 assignfile(f,'bias.bin');
 rewrite(f,sizeof(bufim3));
 blockwrite(f,bufim3,1);
 closefile(f);
 Memo1.Lines.Add('bias.bin записан')
end;

end.

