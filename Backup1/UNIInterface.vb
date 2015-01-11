'------------------------------
'freza.net 2005
'Universal Plugin Connection API
'Created by Garikk
'-----------------------------

Namespace UNI_Universal
    Public Interface UNI_iPluginGetInfo
        'Интерфейс запроса информации о плагине
        ReadOnly Property DCB_GetInfo() As UNI_PluginInfo
    End Interface
    Public Structure UNI_PluginInfo
        'Универсальный список параметров плагина
        Dim INF_NAME As String ' Название плагина
        Dim INF_VER As String ' Версия (major.minor.build.revision)
        Dim INF_PID As String ' стандартный xxSE вызов плагина
        Dim INF_DESCRIPTION As String ' Описание плагина
        Dim INF_OPTIONS As Object ' Дополнительные параметры (привязка на доп. класс)
    End Structure
End Namespace
Namespace UNI_V1
    Public Interface UNI_iPluginConnector
        Function ExecCMD(ByVal Str As String, Optional ByVal Param As Object = Nothing) As Object
        Function PluginInit(ByVal RegPID As String, ByVal BaseSE As UNI_BaseSEConnector) ' RegPID идентификатор выдаваемые менеджером "сервера"

    End Interface
    Public Interface UNI_BaseSEConnector
        Function ExecSECmd(ByVal Str As String, Optional ByVal Param As Object = Nothing, Optional ByVal Params2 As Object = Nothing) As Object
    End Interface
End Namespace
