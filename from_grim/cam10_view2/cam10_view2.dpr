program cam10_view2;

uses
  Forms,
  Unit1 in '..\delphi\Unit1.pas' {Form1};

{$R *.res}

begin
  Application.Initialize;
  Application.CreateForm(TForm1, Form1);
  Application.Run;
end.
