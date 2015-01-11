'------------------------------
'freza.net 2005
'Universal Plugin Connection API
'Created by Garikk
'-----------------------------

Namespace UNI_Universal
    Public Interface UNI_iPluginGetInfo
        '��������� ������� ���������� � �������
        ReadOnly Property DCB_GetInfo() As UNI_PluginInfo
    End Interface
    Public Structure UNI_PluginInfo
        '������������� ������ ���������� �������
        Dim INF_NAME As String ' �������� �������
        Dim INF_VER As String ' ������ (major.minor.build.revision)
        Dim INF_PID As String ' ����������� xxSE ����� �������
        Dim INF_DESCRIPTION As String ' �������� �������
        Dim INF_OPTIONS As Object ' �������������� ��������� (�������� �� ���. �����)
    End Structure
End Namespace
Namespace UNI_V1
    Public Interface UNI_iPluginConnector
        Function ExecCMD(ByVal Str As String, Optional ByVal Param As Object = Nothing) As Object
        Function PluginInit(ByVal RegPID As String, ByVal BaseSE As UNI_BaseSEConnector) ' RegPID ������������� ���������� ���������� "�������"

    End Interface
    Public Interface UNI_BaseSEConnector
        Function ExecSECmd(ByVal Str As String, Optional ByVal Param As Object = Nothing, Optional ByVal Params2 As Object = Nothing) As Object
    End Interface
End Namespace
