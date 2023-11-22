Imports System.Collections.Specialized.BitVector32
Imports System.IO
Imports System.Windows.Documents
Imports Microsoft.VisualBasic.FileIO
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Reflection.Metadata
Imports Paragraph = iTextSharp.text.Paragraph
Imports System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder

Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim filePath As String = "C:\Users\moham\source\repos\finance_manager\File.csv"
        Dim data As String = TextBox1.Text + "," + TextBox2.Text + "," + TextBox3.Text + "," + DateTime.Now
        Using writer As StreamWriter = New StreamWriter(filePath, True)
            writer.WriteLine(data)
        End Using




    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim csvFilePath As String = "C:\Users\moham\source\repos\finance_manager\File.csv"
        Dim csvData As New DataTable()

        Using parser As New TextFieldParser(csvFilePath)
            parser.TextFieldType = FieldType.Delimited
            parser.SetDelimiters(",")

            ' Read the header row to create columns in the DataTable
            Dim headers = parser.ReadFields()
            For Each header In headers
                csvData.Columns.Add(header)
            Next

            ' Read the remaining rows and populate the DataTable
            While Not parser.EndOfData
                Dim fields = parser.ReadFields()
                csvData.Rows.Add(fields)
            End While
        End Using

        DataGridView1.DataSource = csvData

        Dim pdfFilePath As String = "C:\Users\moham\source\repos\finance_manager\File1.pdf"

        ' Read the CSV file into a DataTable
        Dim csvData1 As New DataTable()
        Using parser As New TextFieldParser(csvFilePath)
            parser.TextFieldType = FieldType.Delimited
            parser.SetDelimiters(",")

            ' Read the header row to create columns in the DataTable
            If Not parser.EndOfData Then
                Dim headers = parser.ReadFields()
                For Each header In headers
                    csvData1.Columns.Add(header)
                Next
            End If

            ' Read the remaining rows and populate the DataTable
            While Not parser.EndOfData
                Dim fields = parser.ReadFields()
                csvData1.Rows.Add(fields)
            End While
        End Using

        ' Create a PDF document and add the data from the DataTable
        Using document As New iTextSharp.text.Document()
            Using writer As PdfWriter = PdfWriter.GetInstance(document, New FileStream(pdfFilePath, FileMode.Create))
                document.Open()

                ' Add content to the PDF document
                document.Add(New iTextSharp.text.Paragraph("Hello, this is your finance Report"))
                document.Add(Chunk.NEWLINE) ' Add an empty line

                ' Create a PDF table and populate it with the data from the DataTable
                Dim table As New PdfPTable(csvData1.Columns.Count)
                For Each column As DataColumn In csvData1.Columns
                    table.AddCell(column.ColumnName)
                Next

                For Each row As DataRow In csvData1.Rows
                    For Each cell As Object In row.ItemArray
                        table.AddCell(cell.ToString())
                    Next
                Next

                ' Add the table to the PDF document
                document.Add(table)

                ' Close the PDF document
                document.Close()
            End Using
        End Using


    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DataGridView1.Hide()

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim csvFilePath As String = "C:\Users\moham\source\repos\finance_manager\File.csv"
        Dim csvData As New DataTable()

        Using parser As New TextFieldParser(csvFilePath)
            parser.TextFieldType = FieldType.Delimited
            parser.SetDelimiters(",")

            ' Read the header row to create columns in the DataTable
            Dim headers = parser.ReadFields()
            For Each header In headers
                csvData.Columns.Add(header)
            Next

            ' Read the remaining rows and populate the DataTable
            While Not parser.EndOfData
                Dim fields = parser.ReadFields()
                csvData.Rows.Add(fields)
            End While
        End Using

        DataGridView1.DataSource = csvData
        ' Calculate the sum for each column
        Dim sumColumn1 As Double = 0
        Dim sumColumn2 As Double = 0
        Dim sumColumn3 As Double = 0

        For Each row As DataGridViewRow In DataGridView1.Rows
            If Not row.IsNewRow Then
                Dim value1 As Double
                Dim value2 As Double
                Dim value3 As Double

                If Double.TryParse(Convert.ToString(row.Cells(0).Value), value1) Then
                    sumColumn1 += value1
                End If

                If Double.TryParse(Convert.ToString(row.Cells(1).Value), value2) Then
                    sumColumn2 += value2
                End If

                If Double.TryParse(Convert.ToString(row.Cells(2).Value), value3) Then
                    sumColumn3 += value3
                End If
            End If
        Next

        ' Display the total outside of the DataGridView
        Label5.Text = $"Total:
Total Of Income : {sumColumn1}
Total Of Expenses : {sumColumn2}
Total Of Budgets : {sumColumn3}"

        DataGridView1.Show()

    End Sub
End Class
