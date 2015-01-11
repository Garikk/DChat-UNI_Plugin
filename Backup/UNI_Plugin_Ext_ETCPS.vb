' freza.net 2005
' UNI_Plugin
' ETCPS Types
' Created by Garikk

Imports System.Windows.Forms
Imports System.Drawing

Namespace UNI_Extended
    Public Module UNI_ETCPS
        Public Interface UNI_iETCPS
            Sub ConnectTO(ByVal Addr As String, ByVal Port As Integer)
            Sub NetSend(ByVal Dat As String)
            Sub CloseConnect()
            Sub InitFromSocket(ByVal FromSocket As Net.Sockets.Socket)
            Event ReceiveData(ByVal Dat As String)
            Event NetError(ByVal Ex As Exception)
        End Interface
    End Module
End Namespace

Namespace UNI_DChat
    Namespace DCB_Universal
        '”ниверсальные интерфейсы
        Public Interface DCB_iPlugin
            Function DCB_DCSE_Exec(ByVal PID As String, ByVal CMD As String) As Object
            Sub DCB_DCSE_V1_c_Plugininit(ByRef CHLCTL As DCB_V1.DCB_Channels2.DCB_iChannelsControl, ByRef USRCTL As DCB_V1.DCB_Channels2.DCB_iUsersControl, ByRef DCGUI As DCB_V1.DCB_Base.DCB_GUI_Control, ByRef NETWORK As DCB_V1.DCB_Base.DCB_NetSocket, ByRef DCSE As DCB_V1.DCB_Base.DCB_DCSE)
            ReadOnly Property UNI_GetInfo() As DCB_Universal.DCB_UNI_PluginInfo
        End Interface
        Public Interface DCB_UNI_iPluginGetInfo
            ReadOnly Property DCB_GetInfo() As DCB_Universal.DCB_UNI_PluginInfo
        End Interface
        Public Structure DCB_UNI_PluginInfo
            Dim INF_NAME As String
            Dim INF_VER As Version
            Dim INF_MISCINFO As Hashtable
            Dim INF_CMDEXEC As String
            Dim INF_DESCRIPTION As String
            Dim INF_OPTIONS As DCB_Universal.DCB_Plugin_options
            Dim INF_PLUGINTYPE As DCB_UNI_PluginTypes
            Dim INF_PLUGINTYPESTR As String
            Dim INF_PMGR_PID As Integer
        End Structure
        Public Enum DCB_UNI_PluginTypes
            DCB_V1 = 1  ' »нтерфейс 1 типа
            DCB_V2_m = 2 ' ”нифицированный интерфейс дл€ модулей
            DCB_V2_f = 3 ' ”нифицированный интерфейс дл€ Main-плагинов
        End Enum

        Public Class DCB_LocalUserInfo
            Public UserNick As String
            Public UserAltNick As String
            Public UserCurrentNick As String
            Public UserID As String
            Public RealName As String
            Public UserType As UserTypes
            Public DCB_Profile As String
            Public EMail As String
            Public Enum UserTypes
                USR_ME = 0
                USR_StdRemote = 1
            End Enum
        End Class
        Public Structure DCB_Plugin_options
            Dim DCB_MsgFilter As Boolean
            Dim DCB_UseGUICTL As Boolean
        End Structure

        Public Class DCB_PluginsDB
            Dim MainCollection As New Hashtable
            Public Sub AddPLG(ByVal UDBEntry As DCB_Universal.DCB_iPlugin)
                MainCollection.Add(UDBEntry.UNI_GetInfo.INF_CMDEXEC, UDBEntry)
            End Sub
            Public Sub RemovePLG(ByVal CMDPREF As String)
                MainCollection.Remove(CMDPREF)
            End Sub
            Public Function GetPLG(ByVal Pref As String) As DCB_Universal.DCB_iPlugin
                Return MainCollection.Item(Pref)
            End Function
            Public Function GetMainCollection() As Hashtable
                Return MainCollection
            End Function
        End Class

        Public Structure ChangeRLLayout
            Public Declare Function ActivateKeyboardLayout Lib "user32" (ByVal HKL As Integer, ByVal flags As Integer) As Integer
            Dim B As Boolean

            Public Shared Function ChangeLayout(ByVal InText As String) As String
                Dim enStr As String = "@#$^&QWERTYUIOP{}ASDFGHJKL:" & Chr(34) & "ZXCVBNM<>?qwertyuiop[]asdfghjkl;'zxcvbnm,./" & Chr(34) & "є;:?…÷” ≈Ќ√Ўў«’Џ‘џ¬јѕ–ќЋƒ∆Ёя„—ћ»“№Ѕё,йцукенгшщзхъфывапролдэж€чсмитьбю."
                Dim rusStr As String = Chr(34) & "є;:?…÷” ≈Ќ√Ўў«’Џ‘џ¬јѕ–ќЋƒ∆Ёя„—ћ»“№Ѕё,йцукенгшщзхъфывапролджэ€чсмитьбю." & "@#$^&QWERTYUIOP{}ASDFGHJKL:" & Chr(34) & "ZXCVBNM<>?qwertyuiop[]asdfghjkl;'zxcvbnm,./"
                Dim i As Integer, pos As Integer, temp As String
                For i = 1 To Len(InText)
                    temp = Mid(InText, i, 1)
                    pos = InStr(1, enStr, temp, CompareMethod.Binary)
                    If pos <> 0 Then
                        ChangeLayout = ChangeLayout & Mid(rusStr, pos, 1)
                    Else
                        ChangeLayout = ChangeLayout & temp
                    End If
                Next i
                ActivateKeyboardLayout(1, 1)
                Dim r As Color
            End Function
        End Structure

    End Namespace

    Namespace DCB_V1
        ' »нтерфейс DChat Plugins API V1

        Public Structure DCB_Channels2
            Dim def As String
            ' ¬нешн€€ св€зь класса
            Public Class DCB_Channel
                ' ќсновные
                Dim Intern_CHLName As String
                Dim Intern_CHLID As String
                Dim Intern_CHLTYPE As DCB_Channels2.DCB_Channel_Types
                Dim Intern_PID As String
                Dim Intern_Tag As Object

                Public CHLParams As CHLOptions
                '---------------------------------------------------------------------
#Region " GUI "
                Private Intern_CHLTOPIC As String
                Private Intern_CHLMainMenu As DCB_V1.DCB_Controls.DCB_MenuItem
                Private Intern_tbsCHL As TabPage ' —траничка терминала
                Private Intern_txtCHL As RichTextBox ' контрол RTFCHL
                Private Intern_lstUsers As ListView ' —писок пользователей
                Private Intern_rtflLIST As ListView ' контрол RTFL

                Public Property CHLMainMenu() As DCB_V1.DCB_Controls.DCB_MenuItem
                    Get
                        Return Intern_CHLMainMenu
                    End Get
                    Set(ByVal Value As DCB_V1.DCB_Controls.DCB_MenuItem)
                        Intern_CHLMainMenu = Value
                    End Set
                End Property
                Public Property tbsCHL() As TabPage
                    Get
                        Return Intern_tbsCHL
                    End Get
                    Set(ByVal Value As TabPage)
                        Intern_tbsCHL = Value
                    End Set
                End Property

                Public Property rtflList() As ListView
                    Get
                        Return Intern_rtflLIST
                    End Get
                    Set(ByVal Value As ListView)
                        Intern_rtflLIST = Value
                    End Set
                End Property

                Public Property txtCHL() As RichTextBox
                    Get
                        Return Intern_txtCHL
                    End Get
                    Set(ByVal Value As RichTextBox)
                        Intern_txtCHL = Value
                    End Set
                End Property
                Public Property CHLTOPIC() As String
                    Get
                        Return Intern_CHLTOPIC
                    End Get
                    Set(ByVal Value As String)
                        Intern_CHLTOPIC = Value
                    End Set
                End Property
                Public Property lstUsers() As ListView
                    Get
                        Return Intern_lstUsers
                    End Get
                    Set(ByVal Value As ListView)
                        Intern_lstUsers = Value
                    End Set
                End Property
#End Region
                '---------------------------------------------------------------------
                Sub New(ByVal ChannelName As String, ByVal CHLID As String, ByVal ChannelType As DCB_Channels2.DCB_Channel_Types, ByVal PID As String)
                    Intern_CHLName = ChannelName
                    Intern_CHLID = CHLID
                    Intern_CHLTYPE = ChannelType
                    Intern_PID = PID
                End Sub

                ' ¬нутренние функции
                Public ReadOnly Property CHLName() As String
                    Get
                        Return Intern_CHLName
                    End Get
                End Property

                Public ReadOnly Property CHLID() As String
                    Get
                        Return Intern_CHLID
                    End Get
                End Property
                Public ReadOnly Property CHLTYPE() As DCB_Channels2.DCB_Channel_Types
                    Get
                        Return Intern_CHLTYPE
                    End Get
                End Property

                Public ReadOnly Property PID() As String
                    Get
                        Return Intern_PID
                    End Get
                End Property
                Public Property Tag() As Object
                    Get
                        Return Intern_Tag
                    End Get
                    Set(ByVal Value As Object)
                        Intern_Tag = Value
                    End Set
                End Property

                Protected Overrides Sub Finalize()
                    On Error Resume Next
                    Me.Intern_txtCHL.Dispose()
                    Me.Intern_tbsCHL.Dispose()
                    Me.Intern_lstUsers.Dispose()
                    Me.Intern_rtflLIST.Dispose()

                    MyBase.Finalize()
                End Sub
            End Class

            Public Enum DCB_Channel_Types
                CHL_MainConsole = 0
                CHL_PluginConsole = 1
                CHL_PluginRTFChannel = 2
                CHL_PluginRTFListChannel = 8
                CHL_Other = 9
            End Enum
            Public Structure CHLOptions
                Public USESound As Boolean
                Public USEAlarm As Boolean
            End Structure


            Public Class DCB_User
                ' ѕараметры пользовател€
                Dim intern__internUID As Long
                Dim intern_UserNick As String
                Public UserID As String
                Dim intern_TreeNodes As New Collection
                Public UserCHLs As String
                Dim intern_Creater As String
                Public UserType As DCB_Universal.DCB_LocalUserInfo.UserTypes
                Dim intern_Tag As Object
                Public _miniplugincolorscheme As DCB_minicolorscheme

                Sub New(ByVal UID As String, ByVal UserNick As String, ByVal Tag As Object, ByVal Creater As String)
                    intern__internUID = Rnd() * Long.MaxValue - 5
                    intern_UserNick = UserNick
                    UserID = UID
                    intern_Tag = Tag
                    intern_Creater = Creater
                End Sub
                Public ReadOnly Property _internUID() As Long
                    Get
                        Return intern__internUID
                    End Get
                End Property
                Public ReadOnly Property Creater()
                    Get
                        Return intern_Creater
                    End Get
                End Property
                Public Property UserNick() As String
                    Get
                        Return intern_UserNick
                    End Get
                    Set(ByVal Value As String)
                        Dim TreeNode As ListViewItem
                        For Each TreeNode In intern_TreeNodes
                            TreeNode.Text = TreeNode.Text.Replace(intern_UserNick, Value)
                        Next
                        intern_UserNick = Value
                    End Set
                End Property

                Public Function GetNode(ByVal CHLID As String) As DCB_V1.DCB_Controls.DCB_ListViewItem_TN
                    Try
                        Return intern_TreeNodes(CHLID)
                    Catch ex As Exception
                        Return Nothing
                    End Try
                End Function
                Public Sub AddNodeToTreeview(ByVal DCC As DCB_V1.DCB_Channels2.DCB_Channel, ByVal ListView As ListView, Optional ByVal ImageIndex As Integer = 0)
                    Dim TreeNode As New DCB_V1.DCB_Controls.DCB_ListViewItem_TN
                    TreeNode.Text = intern_UserNick
                    TreeNode.ImageIndex = ImageIndex
                    TreeNode.Tag = DCC.CHLID
                    TreeNode.Info2 = ""
                    TreeNode.Info = ""
                    intern_TreeNodes.Add(TreeNode, TreeNode.Tag)
                    DCC.lstUsers.Items.Add(TreeNode)
                End Sub
                Public Sub DelNodeFromTreeview(ByVal CHLID As String)
                    Try
                        Dim treenode As DCB_V1.DCB_Controls.DCB_ListViewItem_TN = intern_TreeNodes(CHLID)
                        treenode.Remove()
                        intern_TreeNodes.Remove(CHLID)
                    Catch
                    End Try
                End Sub

                Public Property UserTreeViewNodes() As Collection
                    Get
                        Return intern_TreeNodes
                    End Get
                    Set(ByVal Value As Collection)
                        intern_TreeNodes = Value
                    End Set
                End Property
                Protected Overrides Sub Finalize()
                    On Error Resume Next
                    Dim Tmp As Integer
                    For Tmp = 1 To intern_TreeNodes.Count - 1
                        Me.intern_TreeNodes.Remove(Tmp)
                    Next

                    MyBase.Finalize()
                End Sub
            End Class


            Public Structure DCB_minicolorscheme
                Public MyTextColor As Drawing.Color
                Public OurTextColor As Color
            End Structure

#Region " ”правление каналами "
            Public Structure DCB_NewChannelDataPack
                Public CHLID As String
                Public CHLName As String
                Public CHLType As DCB_Channels2.DCB_Channel_Types
                Public tbsTab As TabControl
                Public UlistBase As Panel
                Public iIconBase As ImageList
                Public CHLIcoIndex As Integer
                Public ParentPluginInfo As DCB_Universal.DCB_UNI_PluginInfo
                Sub New(ByVal ParentPluginInfo As DCB_Universal.DCB_UNI_PluginInfo, ByVal CHLID As String, ByVal CHLNAM As String, ByVal CHLType As DCB_Channels2.DCB_Channel_Types, ByVal tbsTab As TabControl, ByVal UListBase As Panel, ByVal IconBase As ImageList, ByVal CHLIcoIndex As Integer)
                    Me.ParentPluginInfo = ParentPluginInfo
                    Me.CHLID = CHLID
                    Me.CHLName = CHLNAM
                    Me.CHLType = CHLType
                    Me.tbsTab = tbsTab
                    Me.UlistBase = UListBase
                    Me.iIconBase = IconBase
                    Me.CHLIcoIndex = CHLIcoIndex
                End Sub
            End Structure

            Public Interface DCB_iChannelsControl
                ' »нтерфейс управлени€ каналом

                ' ”парвление каналом в базе
                Function DCB_AddNewChannel(ByVal NewCHLInfo As DCB_Channels2.DCB_NewChannelDataPack) As DCB_Channels2.DCB_Channel
                Sub DCB_DelCHL(ByVal CHLID As String)
                Function DCB_GetPluginCHLIDs(ByVal PID As String) As String
                Function DCB_GetPluginCHLS(ByVal PID As String) As Collection
                Function DCB_DChannel(ByVal ChlID As String) As DCB_Channels2.DCB_Channel
                Function DCB_DChannelSTR(ByVal ChlNAM As String, ByVal PID As String) As DCB_Channels2.DCB_Channel
                ' ќтображение текста
                Sub DCB_Talk_UUID_ChlDCC(ByVal UID As String, ByVal Channel As DCB_Channels2.DCB_Channel, ByVal MsgType As DCB_Base.DCB_MessageTypes, ByVal Msg As String, ByVal CustomColor As DCB_Base.DCB_IRCColors, Optional ByVal IcoUPD As Byte = 1)
                Sub DCB_Talk_UITEM_CHLNAM(ByVal PID As String, ByVal User As DCB_Channels2.DCB_User, ByVal Channel As String, ByVal MsgType As DCB_Base.DCB_MessageTypes, ByVal Msg As String, ByVal CustomColor As DCB_Base.DCB_IRCColors, Optional ByVal IcoUPD As Byte = 1)
                Sub DCB_Talk_UITEM_CHLDCC(ByVal User As DCB_V1.DCB_Channels2.DCB_User, ByVal Channel As DCB_Channels2.DCB_Channel, ByVal MsgType As DCB_Base.DCB_MessageTypes, ByVal Msg As String, ByVal CustomColor As DCB_Base.DCB_IRCColors, Optional ByVal IcoUPD As Byte = 1)
                Sub DCB_Talk_UUID_CHLID(ByVal UID As String, ByVal CHLID As String, ByVal MsgType As DCB_Base.DCB_MessageTypes, ByVal Msg As String, ByVal CustomColor As DCB_Base.DCB_IRCColors, Optional ByVal IcoUPD As Byte = 1)
                Sub DCB_Talk_UID_ALL_PLUGIN_CHLs(ByVal UID As String, ByVal PluginID As String, ByVal MsgType As DCB_Base.DCB_MessageTypes, ByVal Msg As String, ByVal CustomColor As DCB_Base.DCB_IRCColors, Optional ByVal IcoUPD As Byte = 1)
                Sub DCB_Talk_UID_ALL_PLUGIN_USER_CHLs(ByVal UID As String, ByVal PluginID As String, ByVal MsgType As DCB_Base.DCB_MessageTypes, ByVal Msg As String, ByVal CustomColor As DCB_Base.DCB_IRCColors, Optional ByVal IcoUPD As Byte = 1)

                '
                Sub DCB_JoinUserToChl(ByVal UID As String, ByVal CHLID As String)
                Sub DCB_LeaveUserFromCHL(ByVal UID As String, ByVal CHLID As String)
                Function IsUserExist(ByVal UID As String, ByVal CHLID As String) As Boolean

            End Interface
            Public Interface DCB_iUsersControl
                Function DCB_AddNewUser(ByVal UID As String, ByVal UsrName As String, ByVal Tag As Object, ByVal Creater As String) As DCB_Channels2.DCB_User
                Sub DCB_RemoveUser(ByVal UID As String)
                Sub DCB_RemoveAllPluginUser(ByVal PID As String)
                Function DCB_GetUser_Name(ByVal UsrName As String, ByVal PID As String) As DCB_Channels2.DCB_User
                Function DCB_GetUser_UID(ByVal UID As String) As DCB_Channels2.DCB_User
                Sub DCB_ChangeUserIcon(ByVal UID As String, ByVal IconIndex As Integer, ByVal OnCHL As DCB_Channel)
                Sub DCB_ChangeUserName(ByVal PluginID As String, ByVal UID As String, ByVal NewName As String)
                Function IsCHLExsist(ByVal UID As String, ByVal CHLID As String) As Boolean
            End Interface
#End Region
        End Structure


        Public Structure DCB_Base
            Dim CTL As Boolean
            Public Interface DCB_GUI_Control
                ' »нтерфейс DChat
                Function DCB_GetCurrentChannelDCC() As DCB_Channels2.DCB_Channel
                Function DCB_GetCurrentChannelCHLID() As String
                Function DCB_GetCurrentPluginID() As String
                Function DCB_GetMainForm() As Form
                Function DCB_GetInputTextBox() As RichTextBox
                Function DCB_GetNickTextBox() As RichTextBox
                Function DCB_GetTabControl() As TabControl
                Function DCB_GetListsPanel() As Panel
                Function DCB_GetTray() As Windows.Forms.NotifyIcon
                Function DCB_GetHelpProvider() As HelpProvider
                Function DCB_GetTopicBox() As RichTextBox
                Function DCB_GetToolBarPanel() As Panel
                Sub DCB_UpdateTopicBox()
                Sub DCB_UpdateInterf()
            End Interface

            Public Interface DCB_DCSE
                ' »нтерфейс обработчика комманд DChat, используетс€ дл€ св€зи плагина с Ѕазой
                Function ExecScriptCommand(ByVal PID As String, ByVal CMD As String, Optional ByVal CSeparator As Char = " "c) As Object
                Sub ExecScript(ByVal PID As String, ByVal Path As String, Optional ByVal CallProc As String = "", Optional ByVal Parametrs As String = "")
                '  Sub ExecScriptBlock(ByVal PID As String, ByVal ScriptBlock As String)
                Function GetProcSource(ByVal PID As String, ByVal Path As String, ByVal Proc As String) As String

            End Interface

            Public Interface DCB_NetSocket
                ' »нтерфейс сетевого сокета TCP/UDP
                Sub DCB_NET_NetInit(ByVal PluginID As DCB_Universal.DCB_UNI_PluginInfo)
                Sub DCB_NET_Connect(ByVal URL As String, ByVal Port As Integer)
                Sub DCB_NET_Disconnect()
                Sub DCB_NET_SendMessage(ByVal CMDToSend As String)
                Event DCB_NET_ReciveData(ByVal Dat As String)
                Event DCB_NET_Error(ByVal Err As String)
            End Interface



            Public Enum DCB_MessageTypes
                MSG_DC2_UNAME = 1
                MSG_DC2_NOTUNAME = 2
                MSG_TXT_UNAME = 3
                MSG_TXT_NOTUNAME = 4
                MSG_TXTCOLORED_UNAME = 5
                MSG_TXTCOLORED_NOTUNAME = 6
                MSG_WARNING = 90
                MSG_ERROR = 91
                MSG_CUSTOM = 92
            End Enum
            Public Enum DCB_IRCColors
                White = 0
                Black = 1
                Navy = 2
                Green = 3
                Red = 4
                Maroon = 5
                Purple = 6
                Olive = 7
                Yellow = 8
                Lime = 9
                Teal = 10
                Aqua = 11
                Blue = 12
                Fuchsia = 13
                Gray = 14
                Silver = 15
            End Enum
        End Structure
        Public Structure DCB_Controls
            Dim CTL As Boolean
            Public Class DCB_MenuItem
                Inherits Windows.Forms.MenuItem
                Public CMD As String
                Sub New(ByVal Text As String, ByVal Cmd As String)
                    Me.Text = Text
                    Me.CMD = Cmd
                End Sub
            End Class
            Public Class DCB_ContextMenu
                Inherits Windows.Forms.ContextMenu
                Public CMD As String
            End Class
            Public Class DCB_ListViewItem_TN
                Inherits Windows.Forms.ListViewItem
                Public CMD As String
                Public Info As String
                Public Info2 As String
            End Class
            Public Class DCB_TreeNode
                Inherits Windows.Forms.TreeNode
                Public CMD As String
                Public Info As String
                Public Info2 As String
            End Class

            Public Class DCB_ListViewItem
                Inherits Windows.Forms.ListViewItem
                Public CMD As String
                Public CHLName As String
                Public DC2Text As String
            End Class
            '-----------------------------
        End Structure
        Public Structure DCB_Plugins
            Dim CTL As Boolean

            Public Structure DCB_UNIPluginNotify
                Const DCB_Pluginquit = "DCB_BASEQUIT"
                Const DCB_Pligintest = "DCB_PLUGINTEST"
                Dim Tag As String
            End Structure
            Public Enum DCB_PluginShutdown
                DCB_ProgramShutdown = 1
                DCB_OSShutdown = 2
            End Enum
        End Structure

    End Namespace

    Namespace DCB_V2_m
        ' »нтерфейс дл€ модулей
        Public Structure DCB_Plugins
            Dim CTL As Boolean
            Public Structure DCB_UNIPluginNotify
                Const DCB_Pluginquit = "DCB_BASEQUIT"
                Const DCB_Pligintest = "DCB_PLUGINTEST"
                Dim Tag As String
            End Structure
            Public Enum UNI_PluginShutdown
                UNI_ProgramShutdown = 1
                UNI_OSShutdown = 2
            End Enum
        End Structure

    End Namespace
End Namespace


