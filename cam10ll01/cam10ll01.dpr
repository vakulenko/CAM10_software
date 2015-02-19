// --------------------------------------------------------------------------------
// ASCOM Camera driver low-level interaction library for cam10_v0.1
// Edit Log:
// Date		    	Who	 Vers	  Description
// -----------	---	-----	---------------------------------------------------------
// 17-feb-2015  VSS 0.1     Initial
// --------------------------------------------------------------------------------

library cam10ll01;

uses
  MyD2XX,
  Classes,
  SysUtils,
  Windows;

{$R *.res}

const
cameraWidth = 1280;
cameraHeight = 1024;

cameraIdle = 0;
cameraWaiting = 1;
cameraExposing = 2;
cameraReading = 3;
cameraDownload = 4;
cameraError = 5;

//GLobal variables}
var
//переменная-флаг, отображает состояние соединения с камерой
isConnected : boolean = false;
//переменная-флаг, отображает готовность к считыванию кадра
imageReady : boolean = false;
//переменная-состояние камеры
cameraState : integer = cameraIdle;
//буферный массив-изображение для операций
bufim :array[0..CameraWidth-1,0..CameraHeight-1] of word;

//Connect camera, return bool result
//Опрос подключенных устройств и инициализация
function cameraConnect () : WordBool;  stdcall; export;
begin
 isConnected := true;
 cameraState := cameraIdle;

 Result := true;
end;

//Disconnect camera, return bool result
function cameraDisconnect (): WordBool; stdcall; export;
begin
 isConnected := false;
 cameraState := cameraIdle;

 Result := true;
end;

//Check camera connection, return bool result
function cameraIsConnected () : WordBool; stdcall; export;
begin
  Result := isConnected;
end;

function cameraStartExposure (bin,startX,startY,numX,numY : integer; duration : double; light : WordBool; gain,offset,blevel : integer) : WordBool; stdcall; export;
begin
 imageReady := false;

 cameraState:=cameraWaiting;
 cameraState:=cameraExposing;
 cameraState:=cameraReading;
 cameraState:=cameraDownload;
 cameraState:=cameraIdle;

 imageReady := true;

 Result := true;
end;

//Get camera state, return int result
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

//Check ImageReady flag, is image ready for transfer - transfer image to driver and return bool ImageReady flag
function cameraGetImageReady : WordBool; stdcall; export;
begin
 Result := imageReady;
end;

//Get back pointer to image
function cameraGetImage : dword; stdcall; export;
var i,j : integer;
begin
 for i := 0 to (CameraWidth-1) do
  for j := 0 to (CameraHeight-1) do
    begin
      bufim[i,j]:=i+j;
    end;
 Result := dword(@bufim);
end;

exports cameraConnect;
exports cameraDisconnect;
exports cameraIsConnected;
exports cameraStartExposure;
exports cameraGetCameraState;
exports cameraGetImageReady;
exports cameraGetImage;

begin
end.

