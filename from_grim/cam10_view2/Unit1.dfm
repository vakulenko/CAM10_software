object Form1: TForm1
  Left = 182
  Top = 108
  Width = 965
  Height = 643
  Caption = 'Form1'
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  OnCreate = FormCreate
  OnDestroy = FormClose
  PixelsPerInch = 96
  TextHeight = 13
  object Image1: TImage
    Left = 8
    Top = 8
    Width = 640
    Height = 512
    Cursor = crCross
    Stretch = True
    OnMouseDown = FormMD
  end
  object Label1: TLabel
    Left = 16
    Top = 536
    Width = 35
    Height = 13
    Caption = #1091#1089#1080#1083#1077#1085
  end
  object Label2: TLabel
    Left = 16
    Top = 568
    Width = 41
    Height = 13
    Caption = #1089#1084#1077#1097#1077#1085
  end
  object SpeedButton1: TSpeedButton
    Left = 660
    Top = 16
    Width = 69
    Height = 33
    AllowAllUp = True
    GroupIndex = 1
    Caption = #1086#1090#1082#1088#1099#1090#1100
    OnClick = SpeedButton1Click
  end
  object SpeedButton2: TSpeedButton
    Left = 660
    Top = 56
    Width = 69
    Height = 33
    AllowAllUp = True
    GroupIndex = 2
    Caption = #1086#1087#1088#1086#1089
    Enabled = False
    OnClick = SpeedButton2Click
  end
  object Image2: TImage
    Left = 684
    Top = 456
    Width = 256
    Height = 137
  end
  object Label3: TLabel
    Left = 512
    Top = 531
    Width = 31
    Height = 13
    Caption = #1040#1076#1088#1077#1089
  end
  object Label4: TLabel
    Left = 584
    Top = 531
    Width = 48
    Height = 13
    Caption = #1047#1085#1072#1095#1077#1085#1080#1077
  end
  object Image3: TImage
    Left = 741
    Top = 248
    Width = 200
    Height = 200
    Stretch = True
  end
  object Label5: TLabel
    Left = 744
    Top = 194
    Width = 68
    Height = 13
    Caption = #1101#1082#1089#1087#1086#1079', '#1084#1089#1077#1082
  end
  object SpeedButton3: TSpeedButton
    Left = 424
    Top = 536
    Width = 49
    Height = 24
    AllowAllUp = True
    GroupIndex = 3
    Caption = #1079#1077#1088#1082
    OnClick = SpeedButton3Click
  end
  object SpeedButton4: TSpeedButton
    Left = 664
    Top = 368
    Width = 65
    Height = 29
    Caption = #1092#1072#1081#1083' '#1073#1080#1072#1089
  end
  object SpeedButton5: TSpeedButton
    Left = 664
    Top = 326
    Width = 65
    Height = 29
    Caption = #1092#1072#1081#1083' '#1074#1080#1076#1077#1086
  end
  object Memo1: TMemo
    Left = 740
    Top = 16
    Width = 205
    Height = 161
    ScrollBars = ssVertical
    TabOrder = 0
    OnDblClick = Memo1Change
  end
  object Button4: TButton
    Left = 828
    Top = 194
    Width = 93
    Height = 39
    Caption = #1082#1072#1076#1088
    Enabled = False
    TabOrder = 1
    OnClick = Button4Click
  end
  object TrackBar1: TTrackBar
    Left = 64
    Top = 536
    Width = 353
    Height = 24
    Enabled = False
    Max = 63
    Position = 63
    TabOrder = 2
    ThumbLength = 10
    OnChange = TrackBar1Change
  end
  object TrackBar2: TTrackBar
    Left = 64
    Top = 568
    Width = 353
    Height = 24
    Enabled = False
    Max = 63
    Min = -63
    TabOrder = 3
    ThumbLength = 10
    OnChange = TrackBar2Change
  end
  object Button1: TButton
    Left = 660
    Top = 290
    Width = 69
    Height = 25
    Caption = #1092#1072#1081#1083' out'
    TabOrder = 4
    OnClick = Button1Click
  end
  object SpinEdit1: TSpinEdit
    Left = 752
    Top = 210
    Width = 57
    Height = 22
    MaxValue = 2000
    MinValue = 1
    TabOrder = 5
    Value = 5
    OnChange = SpinEdit1Change
  end
  object Edit1: TEdit
    Left = 504
    Top = 547
    Width = 49
    Height = 21
    TabOrder = 6
    Text = '$01'
  end
  object Button2: TButton
    Left = 504
    Top = 571
    Width = 57
    Height = 25
    Caption = #1095#1090#1077#1085#1080#1077
    TabOrder = 7
    OnClick = Button2Click
  end
  object Button3: TButton
    Left = 584
    Top = 571
    Width = 57
    Height = 25
    Caption = #1079#1072#1087#1080#1089#1100
    TabOrder = 8
    OnClick = Button3Click
  end
  object Edit2: TEdit
    Left = 584
    Top = 547
    Width = 49
    Height = 21
    TabOrder = 9
    Text = '$0000'
  end
  object CheckBox2: TCheckBox
    Left = 424
    Top = 568
    Width = 65
    Height = 17
    Caption = #1072#1074#1090#1086' '#1089#1084
    Enabled = False
    TabOrder = 10
    OnClick = CheckBox2Click
  end
  object CheckBox1: TCheckBox
    Left = 664
    Top = 146
    Width = 57
    Height = 17
    Caption = '-bias'
    TabOrder = 11
  end
  object RadioGroup1: TRadioGroup
    Left = 656
    Top = 176
    Width = 73
    Height = 65
    Caption = 'iso'
    ItemIndex = 0
    Items.Strings = (
      '0..7'
      '0..6'
      '0..5')
    TabOrder = 12
  end
  object CheckBox3: TCheckBox
    Left = 664
    Top = 96
    Width = 57
    Height = 17
    Caption = 'ROI'
    TabOrder = 13
  end
  object CheckBox4: TCheckBox
    Left = 664
    Top = 120
    Width = 57
    Height = 17
    Caption = 'bin 2*2'
    TabOrder = 14
  end
  object Edit3: TEdit
    Left = 656
    Top = 256
    Width = 73
    Height = 21
    TabOrder = 15
    Text = 'NoName'
  end
  object Timer1: TTimer
    Enabled = False
    Interval = 10
    OnTimer = Timer1Timer
    Left = 652
    Top = 440
  end
end
