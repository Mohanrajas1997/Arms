﻿Imports System.IO
Imports System.IO.FileStream
Imports MySql.Data.MySqlClient
Imports System.Data

Public Class frmChequeUpdate

#Region "Local Declaration"
    Dim lnChqId As Integer
    Dim lnDiscVal As Integer
    Dim lnResult As Long
    Dim lsIpAddr As String = ""
    Dim lsContractNo As String = ""
    Dim ldChqAmount As Double = 0
    Dim lsChqNo As String = ""
    Dim lsCycleDate As String = ""
    Dim lnEntityGid As Integer
    Dim lsChqDate As String = ""
    Dim cmd As MySqlCommand
    Dim da As MySqlDataAdapter
    Dim dt As DataTable
#End Region

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.       
    End Sub

    Private Sub fp_DiscValue()
        Dim i As Integer

        gnDiscVal = 0
        For i = 0 To DGrid.RowCount - 1
            If Not IsDBNull(DGrid.Item(2, i).Value) Then
                If DGrid.Item(2, i).Value = True Then
                    gnDiscVal += DGrid.Item(1, i).Value
                End If
            End If
        Next
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Try
            Call fp_DiscValue()

            If txtChqId.Text.Trim = "" Then
                MsgBox("Chq Id cannot be empty !", MsgBoxStyle.Critical, gsProjectName)
                txtChqId.Focus()
            Else

                Dim i As Integer = 0
                If Not Integer.TryParse(txtChqId.Text, i) Then
                    i = 0
                End If
                lnChqId = i
                lsContractNo = txtContractNo.Text
                lsCycleDate = Format(CDate(dtpCycleDate.Text), "yyyy-MM-dd")
                lsChqDate = Format(CDate(dtpChqDate.Text), "yyyy-MM-dd")
                ldChqAmount = txtChqAmount.Text
                lnDiscVal = gnDiscVal
                lsChqNo = Mid(Format(Val(QuoteFilter(txtChqNo.Text).ToString), "000000"), 1, 16)
                If Val(lsChqNo) = 0 Then lsChqNo = ""

                Call ConOpenOdbc(ServerDetails)
                cmd = New MySqlCommand("pr_arms_set_updatechq", gOdbcConn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("?pr_chq_gid", lnChqId)
                cmd.Parameters("?pr_chq_gid").Direction = ParameterDirection.Input
                cmd.Parameters.AddWithValue("?pr_contract_no", lsContractNo)
                cmd.Parameters("?pr_contract_no").Direction = ParameterDirection.Input
                cmd.Parameters.AddWithValue("?pr_cycle_date", CDate(lsCycleDate))
                cmd.Parameters("?pr_cycle_date").Direction = ParameterDirection.Input
                cmd.Parameters.AddWithValue("?pr_chq_date", CDate(lsChqDate))
                cmd.Parameters("?pr_chq_date").Direction = ParameterDirection.Input
                cmd.Parameters.AddWithValue("?pr_chq_no", lsChqNo)
                cmd.Parameters("?pr_chq_no").Direction = ParameterDirection.Input
                cmd.Parameters.AddWithValue("?pr_chq_amount", ldChqAmount)
                cmd.Parameters("?pr_chq_amount").Direction = ParameterDirection.Input
                cmd.Parameters.AddWithValue("?pr_action_by", gsLoginUserCode)
                cmd.Parameters("?pr_action_by").Direction = ParameterDirection.Input
                cmd.Parameters.AddWithValue("?pr_chq_disc", lnDiscVal)
                cmd.Parameters("?pr_chq_disc").Direction = ParameterDirection.Input
                'out put Para
                cmd.Parameters.Add("?pr_result", MySqlDbType.Int32)
                cmd.Parameters("?pr_result").Direction = ParameterDirection.Output
                cmd.Parameters.Add("?pr_msg", MySqlDbType.VarChar)
                cmd.Parameters("?pr_msg").Direction = ParameterDirection.Output
                cmd.ExecuteNonQuery()
                Call ConCloseOdbc(ServerDetails)
                Dim res As Integer = Val(cmd.Parameters("?pr_result").Value.ToString())
                If (res = 0) Then
                    MsgBox(cmd.Parameters("?pr_msg").Value.ToString())
                    Call ControleEnable()
                Else
                    MsgBox(cmd.Parameters("?pr_msg").Value.ToString())
                    Call Clear()
                    lnChqId = 0
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try


    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        Call Clear()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        If MsgBox("Do you want to Close?", MsgBoxStyle.YesNo + MsgBoxStyle.Question + MsgBoxStyle.DefaultButton2) = MsgBoxResult.Yes Then
            MyBase.Close()
        End If
    End Sub

    Private Sub frmChequeUpdate_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Call Clear()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Clear()

        txtChqId.Enabled = True
        txtChqId.Text = ""

        cboEntity.Enabled = False
        txtContractNo.Enabled = False
        txtChqNo.Enabled = False
        dtpChqDate.Enabled = False
        dtpCycleDate.Enabled = False
        txtChqAmount.Enabled = False

        cboEntity.Text = ""
        txtContractNo.Text = ""
        txtChqNo.Text = ""
        dtpChqDate.Text = ""
        txtChqAmount.Text = ""
        dtpCycleDate.Text = ""

        txtChqId.Focus()

        DGrid.DataSource = Nothing

    End Sub

    Private Sub ControleEnable()

        txtChqId.Enabled = False

        cboEntity.Enabled = False
        txtContractNo.Enabled = False
        txtChqNo.Enabled = False
        dtpChqDate.Enabled = False
        dtpCycleDate.Enabled = False
        txtChqAmount.Enabled = False

        cboEntity.Focus()

    End Sub

    Public Sub PresetSelectedValue(ByRef ComboBox As ComboBox, ByVal value As Object)
        Dim item_ndx As Integer

        If ComboBox Is Nothing Then
            '   throw exception?
            Exit Sub
        End If
        With ComboBox
            .Tag = "PresetSelectedValue"
            For item_ndx = 0 To .Items.Count - 1
                .SelectedIndex = item_ndx
                If .SelectedValue = value Then
                    Exit For
                End If
            Next
            If item_ndx >= .Items.Count Then
                .SelectedIndex = -1
            End If
            .Tag = ""
        End With
    End Sub

    Private Sub txtChqId_KeyDown(sender As Object, e As KeyEventArgs) Handles txtChqId.KeyDown
        If e.KeyCode = Keys.Enter Then

            Dim i As Integer
            If Not Integer.TryParse(txtChqId.Text, i) Then
                i = 0
            End If
            lnChqId = i

            cmd = New MySqlCommand("pr_arms_get_cheque", gOdbcConn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("?pr_chq_gid", lnChqId)
            cmd.Parameters("?pr_chq_gid").Direction = ParameterDirection.Input
            dt = New DataTable
            da = New MySqlDataAdapter(cmd)
            da.Fill(dt)

            If (dt.Rows.Count <> 0) Then

                If (dt.Rows(0)("batch_gid") = 0) Then

                    lnEntityGid = dt.Rows(0)("entity_gid")
                    lsContractNo = dt.Rows(0)("contract_no")
                    lsCycleDate = dt.Rows(0)("cycle_date")
                    lsChqDate = dt.Rows(0)("chq_date")
                    ldChqAmount = dt.Rows(0)("chq_amount")
                    lsChqNo = dt.Rows(0)("chq_no")
                    gnDiscVal = dt.Rows(0)("chq_disc")

                    gsQry = " select CONCAT(entity_code,'-', entity_name) as EntityCode ,entity_gid from arms_mst_tentity"
                    gsQry &= " where delete_flag='N' "
                    gsQry &= " order by EntityCode "

                    Call gpBindCombo(gsQry, "EntityCode", "entity_gid", cboEntity, gOdbcConn)

                    Call PresetSelectedValue(cboEntity, lnEntityGid)

                    txtContractNo.Text = lsContractNo
                    dtpCycleDate.Text = lsCycleDate
                    dtpChqDate.Text = lsChqDate
                    txtChqAmount.Text = ldChqAmount
                    txtChqNo.Text = lsChqNo

                    Call ControleEnable()

                    Call fp_BindGrid()
                Else
                    MsgBox("Access Denied!")
                    Clear()
                End If
            Else
                MsgBox("Sorry Chq Id Not Matched!")
                Clear()
            End If

        End If
    End Sub


    Sub fp_BindGrid()
        Dim lobjDT As DataTable
        Dim objDataColumn As DataColumn

        Dim i As Integer
        Try

            gsQry = ""
            gsQry &= " SELECT disc_desc 'Disc', disc_flag "
            gsQry &= " FROM arms_mst_tdisc "
            gsQry &= " where delete_flag = 'N' "
            gsQry &= " order by disc_flag asc "

            lobjDT = GetDataTable(gsQry)
            If lobjDT.Rows.Count > 0 Then
                objDataColumn = New DataColumn
                objDataColumn.DataType = System.Type.GetType("System.Boolean")
                objDataColumn.ColumnName = "Select"

                lobjDT.Columns.Add(objDataColumn)
                DGrid.DataSource = lobjDT

                DGrid.Columns(0).ReadOnly = True
                DGrid.Columns(1).Visible = False

                DGrid.Columns(0).Width = 300
                DGrid.Columns(2).Width = 50
            End If

            If gnDiscVal <> 0 Then
                For i = 0 To DGrid.RowCount - 1
                    If (DGrid.Item(1, i).Value And gnDiscVal) > 0 Then
                        DGrid.Item(2, i).Value = True
                    Else
                        DGrid.Item(2, i).Value = False
                    End If
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, gsProjectName)
        End Try

    End Sub
End Class