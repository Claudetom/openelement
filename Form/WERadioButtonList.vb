﻿Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Text
Imports System.Web.UI

Imports openElement.WebElement.Common
Imports openElement.WebElement.Common.Attributes
Imports openElement.WebElement.Editors.Converter
Imports openElement.WebElement.Elements
Imports openElement.WebElement.ElementWECommon.Form.Forms
Imports openElement.WebElement.StylesManager
Imports openElement.WebElement.StylesManager.CssItems
Imports openElement.WebElement.StylesManager.CssItems.Converter

Imports WebElement.Elements.Form.Editors
Imports WebElement.Elements.Form.Editors.Converter
Imports WebElement.My.Resources.text
Imports WebElement.Ressource.localizable

Namespace Elements.Form

    <Serializable, _
    OEObsolete(1, 31)> _
    Public Class WERadioButtonList
        Inherits ElementBase

        #Region "Fields"

        Private _Disposition As WERadioButtonListDisposition
        Private _RadioElement As FrmRadioListElements.WERadioButtonListElement
        Private _Title As LocalizableHtml
        Private _TitlePosition As TextPosition

        #End Region 'Fields

        #Region "Constructors"

        Public Sub New(ByVal page As openElement.WebElement.Page, ByVal parentID As String, ByVal templateName As String)
            MyBase.New(EnuElementType.PageEdit, "WERadioButtonList", page, parentID, templateName)
            MyBase.TypeResize = EnuTypeResize.None
            Me.TitlePosition = TextPosition.leftmiddle
        End Sub

        #End Region 'Constructors

        #Region "Properties"

        <Ressource.localizable.LocalizableCatAtt(Ressource.localizable.LocalizableCatAtt.EnumWECategory.Appearance), _
        Ressource.localizable.LocalizableNameAtt("_N025"), _
        LocalizableDescAtt("_D067"), _
        Editor(GetType(UITypeRadioListDisposition), GetType(UITypeEditor)), _
        PageUpdateMode(PageUpdateMode.EnuUpdateMode.Element), _
        TypeConverter(GetType(TConvWERadioButtonListDisposition))> _
        Public Property Disposition() As WERadioButtonListDisposition
            Get
                If _Disposition Is Nothing Then
                    _Disposition = New WERadioButtonListDisposition()
                    If _Disposition.Alignement <> CssEnum.TextAlign.novalue Then MyBase.StylesSkin.BaseDiv.BaseStyles.TextAlign = _Disposition.Alignement
                End If
                Return _Disposition
            End Get
            Set(ByVal value As WERadioButtonListDisposition)
                _Disposition = value
                MyBase.StylesSkin.BaseDiv.BaseStyles.TextAlign = value.Alignement
            End Set
        End Property

        <Ressource.localizable.LocalizableCatAtt(Ressource.localizable.LocalizableCatAtt.EnumWECategory.Edition), _
        Ressource.localizable.LocalizableNameAtt("_N065"), _
        LocalizableDescAtt("_D065"), _
        PageUpdateMode(PageUpdateMode.EnuUpdateMode.ElementWithCss), _
        Editor(GetType(UITypeRadioListElements), GetType(UITypeEditor)), _
        TypeConverter(GetType(TConvRadioListElement))> _
        Public Property RadioElement() As FrmRadioListElements.WERadioButtonListElement
            Get
                If _RadioElement Is Nothing Then _RadioElement = New FrmRadioListElements.WERadioButtonListElement()
                Return _RadioElement
            End Get
            Set(ByVal value As FrmRadioListElements.WERadioButtonListElement)
                _RadioElement = value
            End Set
        End Property

        <Browsable(False), _
        ExportVar(ExportVar.EnuVarType.Php), _
        TypeConverter(GetType(TConvLocalizableString)), _
        PageUpdateMode(PageUpdateMode.EnuUpdateMode.Element)> _
        Public Property Title() As LocalizableHtml
            Get
                If _Title Is Nothing Then
                    _Title = New LocalizableHtml(LocalizablePropertyDefaultValue._0007) 'Nom du champs:
                End If
                Return _Title
            End Get
            Set(ByVal value As LocalizableHtml)
                _Title = value
            End Set
        End Property

        <Ressource.localizable.LocalizableCatAtt(Ressource.localizable.LocalizableCatAtt.EnumWECategory.Appearance), _
        Ressource.localizable.LocalizableNameAtt("_N045"), _
        LocalizableDescAtt("_D066"), _
        PageUpdateMode(PageUpdateMode.EnuUpdateMode.ElementWithCss)> _
        Public Property TitlePosition() As TextPosition
            Get
                Return _TitlePosition
            End Get
            Set(ByVal value As TextPosition)
                _TitlePosition = value
            End Set
        End Property

        #End Region 'Properties

        #Region "Methods"

        Protected Overrides Function OnGetInfo() As ElementInfo
            Dim info As New ElementInfo(Me)
            info.ToolBoxCaption = LocalizableOpen._0095
            info.VersionMajor = 1
            info.VersionMinor = 0
            info.GroupName = "NBGroupForm"
            info.ToolBoxIco = My.Resources.WERadioButtonList
            info.ToolBoxDescription = LocalizableOpen._0096
            info.AutoOpenProperty = "RadioElement"
            info.SortPropertyList.Add(New SortProperty("RadioElement", "DataTable.png", LocalizableOpen._0099))
            info.SortPropertyList.Add(New SortProperty("Disposition", "position.png", LocalizableOpen._0100))
            Return info
        End Function

        Protected Overrides Sub OnOpen()
            Dim configStylesZones As New List(Of ConfigStylesZone)
            configStylesZones.Add(New ConfigStylesZone("SpanList", LocalizableOpen._0097, LocalizableOpen._0098))
            configStylesZones.Add(New ConfigStylesZone("ZoneTitre", LocalizableOpen._0079, LocalizableOpen._0080))
            configStylesZones.Add(New ConfigStylesZone("RadioButton", LocalizableOpen._0117, LocalizableOpen._0118))

            MyBase.OnOpen(configStylesZones)
        End Sub

        Protected Overrides Sub Render(ByVal writer As HtmlWriter)
            MyBase.RenderBeginTag(writer)

            'Ajout du titre par rapport à sa position
            Select Case TitlePosition

                Case TextPosition.top
                    'writer.WriteBeginTag("div")
                    ''writer.WriteAttribute("class", MyBase.GetStyleZoneClass("ZoneTitre"))
                    'MyBase.RenderBeginTextEdit(writer, "Title", False, False, True, MyBase.GetStyleZoneClass("ZoneTitre"))
                    'writer.Write(System.Web.UI.HtmlTextWriter.TagRightChar)
                    'Call Me.RenderTitle(writer)
                    'writer.WriteEndTag("div")

                    Dim attr As New Dictionary(Of String, String)
                    attr.Add("class", MyBase.GetStyleZoneClass("ZoneTitre"))
                    writer.WriteHtmlBlockEdit(Me, "Title", False, attr)

                    Call RenderContenu(writer)

                Case TextPosition.bottom

                    Call RenderContenu(writer)

                    'writer.WriteBeginTag("div")
                    ''writer.WriteAttribute("class", MyBase.GetStyleZoneClass("ZoneTitre"))
                    'MyBase.RenderBeginTextEdit(writer, "Title", False, False, True, MyBase.GetStyleZoneClass("ZoneTitre"))
                    'writer.Write(System.Web.UI.HtmlTextWriter.TagRightChar)
                    'Call Me.RenderTitle(writer)
                    'writer.WriteEndTag("div")

                    Dim attr As New Dictionary(Of String, String)
                    attr.Add("class", MyBase.GetStyleZoneClass("ZoneTitre"))
                    writer.WriteHtmlBlockEdit(Me, "Title", False, attr)

                Case TextPosition.rightbottom
                    Call RenderTableTitle(writer, "vertical-align:bottom;", "right")

                Case TextPosition.rightmiddle
                    Call RenderTableTitle(writer, "vertical-align:middle;", "right")

                Case TextPosition.righttop
                    Call RenderTableTitle(writer, "vertical-align:top;", "right")

                Case TextPosition.leftbottom
                    Call RenderTableTitle(writer, "vertical-align:bottom;", "left")

                Case TextPosition.leftmiddle
                    Call RenderTableTitle(writer, "vertical-align:middle;", "left")

                Case TextPosition.lefttop
                    Call RenderTableTitle(writer, "vertical-align:top;", "left")

            End Select
            MyBase.RenderEndTag(writer)
            writer.WriteLine()
            MyBase.StylesSkin.BaseDiv.BaseStyles.Direction = CssEnum.Direction.novalue
        End Sub

        'Render du contenu même le la liste
        Private Sub RenderContenu(ByRef writer As HtmlWriter, Optional ByVal withDiv As Boolean = True)
            If withDiv Then
                writer.WriteBeginTag("div")
                writer.WriteAttribute("class", MyBase.GetStyleZoneClass("SpanList"))
                writer.Write(HtmlTextWriter.TagRightChar)
            End If
            Select Case Disposition.Disposition
                Case RadioButtonDisposition.horizontal

                    Select Case Disposition.LabelsPosition
                        Case TextPosition.bottom
                            For Each element As Object() In RadioElement.RadioList
                                Dim champs As LocalizableString = CType(element(0), LocalizableString)
                                Dim value As LocalizableString = CType(element(1), LocalizableString)
                                Dim checked As Boolean = CType(element(2), Boolean)

                                Call RenderHorizontalLabelWidthStyle(writer)
                                Call RenderInput(writer, value, checked)
                                writer.WriteBreak()
                                writer.Write(champs.GetValue(MyBase.Page.Culture))
                                writer.WriteEndTag("span")
                                writer.WriteLine()
                            Next
                        Case TextPosition.leftbottom, TextPosition.leftmiddle, TextPosition.lefttop
                            For Each element As Object() In RadioElement.RadioList
                                Dim champs As LocalizableString = CType(element(0), LocalizableString)
                                Dim value As LocalizableString = CType(element(1), LocalizableString)
                                Dim checked As Boolean = CType(element(2), Boolean)

                                Call RenderHorizontalLabel(writer)
                                writer.Write(champs.GetValue(MyBase.Page.Culture))
                                Call RenderInput(writer, value, checked)
                                writer.WriteEndTag("span")
                                writer.WriteLine()
                            Next
                        Case TextPosition.rightbottom, TextPosition.rightmiddle, TextPosition.righttop
                            For Each element As Object() In RadioElement.RadioList
                                Dim champs As LocalizableString = CType(element(0), LocalizableString)
                                Dim value As LocalizableString = CType(element(1), LocalizableString)
                                Dim checked As Boolean = CType(element(2), Boolean)

                                Call RenderHorizontalLabel(writer)
                                Call RenderInput(writer, value, checked)
                                writer.Write(champs.GetValue(MyBase.Page.Culture))
                                writer.WriteEndTag("span")
                                writer.WriteLine()
                            Next
                        Case TextPosition.top
                            For Each element As Object() In RadioElement.RadioList
                                Dim champs As LocalizableString = CType(element(0), LocalizableString)
                                Dim value As LocalizableString = CType(element(1), LocalizableString)
                                Dim checked As Boolean = CType(element(2), Boolean)

                                Call RenderHorizontalLabelWidthStyle(writer)
                                writer.Write(champs.GetValue(MyBase.Page.Culture))
                                writer.WriteBreak()
                                Call RenderInput(writer, value, checked)
                                writer.WriteEndTag("span")
                                writer.WriteLine()
                            Next
                    End Select

                Case RadioButtonDisposition.Vertical
                    Select Case Disposition.LabelsPosition
                        Case TextPosition.bottom
                            For Each element As Object() In RadioElement.RadioList
                                Dim champs As LocalizableString = CType(element(0), LocalizableString)
                                Dim value As LocalizableString = CType(element(1), LocalizableString)
                                Dim checked As Boolean = CType(element(2), Boolean)

                                Call RenderVerticalLabel(writer)
                                Call RenderInput(writer, value, checked)
                                writer.WriteBreak()
                                writer.Write(champs.GetValue(MyBase.Page.Culture))
                                writer.WriteEndTag("span")
                                writer.WriteLine()
                            Next
                        Case TextPosition.leftbottom, TextPosition.leftmiddle, TextPosition.lefttop
                            For Each element As Object() In RadioElement.RadioList
                                Dim champs As LocalizableString = CType(element(0), LocalizableString)
                                Dim value As LocalizableString = CType(element(1), LocalizableString)
                                Dim checked As Boolean = CType(element(2), Boolean)

                                Call RenderVerticalLabel(writer)
                                writer.Write(champs.GetValue(MyBase.Page.Culture))
                                Call RenderInput(writer, value, checked)
                                writer.WriteEndTag("span")
                                writer.WriteLine()
                            Next
                        Case TextPosition.rightbottom, TextPosition.rightmiddle, TextPosition.righttop
                            For Each element As Object() In RadioElement.RadioList
                                Dim champs As LocalizableString = CType(element(0), LocalizableString)
                                Dim value As LocalizableString = CType(element(1), LocalizableString)
                                Dim checked As Boolean = CType(element(2), Boolean)

                                Call RenderVerticalLabel(writer)
                                Call RenderInput(writer, value, checked)
                                writer.Write(champs.GetValue(MyBase.Page.Culture))
                                writer.WriteEndTag("span")
                                writer.WriteLine()
                            Next
                        Case TextPosition.top
                            For Each element As Object() In RadioElement.RadioList
                                Dim champs As LocalizableString = CType(element(0), LocalizableString)
                                Dim value As LocalizableString = CType(element(1), LocalizableString)
                                Dim checked As Boolean = CType(element(2), Boolean)

                                Call RenderVerticalLabel(writer)
                                writer.Write(champs.GetValue(MyBase.Page.Culture))
                                writer.WriteBreak()
                                Call RenderInput(writer, value, checked)
                                writer.WriteEndTag("span")
                                writer.WriteLine()
                            Next

                    End Select

                Case RadioButtonDisposition.verticalAlign
                    Select Case Disposition.LabelsPosition
                        Case TextPosition.leftbottom, TextPosition.leftmiddle, TextPosition.lefttop
                            Call RenderVerticalAlignLeft(writer)

                        Case TextPosition.rightbottom, TextPosition.rightmiddle, TextPosition.righttop
                            Call RenderVerticalAlignRight(writer)

                    End Select

            End Select

            If withDiv Then writer.WriteEndTag("div")
        End Sub

        Private Sub RenderHorizontalLabel(ByVal writer As HtmlWriter)
            writer.WriteBeginTag("span")
            Dim labelBuilder As New StringBuilder()
            Select Case Disposition.Alignement
                Case CssEnum.TextAlign.right
                    labelBuilder.Append(StylesUtils.ConcatCSSValue("text-align:", "right", ";"))
                Case CssEnum.TextAlign.center
                    labelBuilder.Append(StylesUtils.ConcatCSSValue("text-align:", "center", ";"))
            End Select
            writer.WriteAttribute("style", labelBuilder.ToString)
            writer.Write(HtmlTextWriter.TagRightChar)
        End Sub

        Private Sub RenderHorizontalLabelWidthStyle(ByVal writer As HtmlWriter)
            Dim labelBuilder As New StringBuilder()
            writer.WriteBeginTag("span")
            labelBuilder.Append(StylesUtils.ConcatCSSValue("display:", "block", ";"))
            labelBuilder.Append(StylesUtils.ConcatCSSValue("float:", "left", ";"))
            Select Case Disposition.Alignement
                Case CssEnum.TextAlign.right
                    labelBuilder.Append(StylesUtils.ConcatCSSValue("text-align:", "right", ";"))
                Case CssEnum.TextAlign.center
                    labelBuilder.Append(StylesUtils.ConcatCSSValue("text-align:", "center", ";"))
            End Select
            labelBuilder.Append(StylesUtils.ConcatCSSValue("margin:", "0px 5px 0px 5px", ";")) 'Ajouter une margin-right:5px à la class de départ
            writer.WriteAttribute("style", labelBuilder.ToString)
            writer.Write(HtmlTextWriter.TagRightChar)
        End Sub

        Private Sub RenderInput(ByVal writer As HtmlWriter, ByVal value As LocalizableString, ByVal checked As Boolean)
            writer.WriteBeginTag("input")
            writer.WriteAttribute("type", "radio")
            writer.WriteAttribute("class", MyBase.GetStyleZoneClass("RadioButton"))
            writer.WriteAttribute("name", Me.ID)
            writer.WriteAttribute("value", value.GetValue(MyBase.Page.Culture))
            If checked Then writer.WriteAttribute("checked", "checked")
            writer.Write("/>")
        End Sub

        Private Sub RenderTableTitle(ByRef writer As HtmlWriter, ByVal styleTD As String, ByVal order As String)
            writer.WriteBeginTag("table")
            writer.WriteAttribute("style", "padding:0px; border-collapse:collapse; border-spacing: 0px;")
            writer.Write(HtmlTextWriter.TagRightChar)

            writer.WriteBeginTag("tr")
            writer.Write(HtmlTextWriter.TagRightChar)
            If order = "left" Then
                writer.WriteBeginTag("td")
                'writer.WriteAttribute("class", MyBase.GetStyleZoneClass("ZoneTitre"))
                writer.WriteAttribute("style", styleTD)
                writer.WriteAttribute("class", MyBase.GetStyleZoneClass("ZoneTitre"))
                'MyBase.RenderBeginTextEdit(writer, "Title", False, False, True, MyBase.GetStyleZoneClass("ZoneTitre"))
                writer.Write(HtmlTextWriter.TagRightChar)
                'Call Me.RenderTitle(writer)
                writer.WriteHtmlBlockEdit(Me, "Title", False, , HtmlWriter.BlockType.MaxBox)
                writer.WriteEndTag("td")

                writer.WriteBeginTag("td")
                writer.WriteAttribute("class", MyBase.GetStyleZoneClass("SpanList"))
                writer.Write(HtmlTextWriter.TagRightChar)
                Call RenderContenu(writer, False)
                writer.WriteEndTag("td")

            Else
                writer.WriteBeginTag("td")
                writer.WriteAttribute("class", MyBase.GetStyleZoneClass("SpanList"))
                writer.Write(HtmlTextWriter.TagRightChar)
                Call RenderContenu(writer, False)
                writer.WriteEndTag("td")

                writer.WriteBeginTag("td")
                'writer.WriteAttribute("class", MyBase.GetStyleZoneClass("ZoneTitre"))
                writer.WriteAttribute("style", styleTD)
                writer.WriteAttribute("class", MyBase.GetStyleZoneClass("ZoneTitre"))
                'MyBase.RenderBeginTextEdit(writer, "Title", False, False, True, MyBase.GetStyleZoneClass("ZoneTitre"))
                writer.Write(HtmlTextWriter.TagRightChar)
                writer.WriteHtmlBlockEdit(Me, "Title", False, , HtmlWriter.BlockType.MaxBox)
                'Call Me.RenderTitle(writer)
                writer.WriteEndTag("td")

            End If
            writer.WriteEndTag("tr")
            writer.WriteEndTag("table")
            writer.WriteLine()
        End Sub

        Private Sub RenderVerticalAlignLeft(ByRef writer As HtmlWriter)
            If String.IsNullOrEmpty(MyBase.StylesSkin.BaseDiv.BaseStyles.Width.ToCss) Then MyBase.StylesSkin.BaseDiv.BaseStyles.Width.SetCss("250px")

            Dim tdBuiler As New StringBuilder()
            tdBuiler.Append(StylesUtils.ConcatCSSValue("text-align:", "right", ";"))

            For Each element As Object In RadioElement.RadioList
                Call RenderVerticalLabel(writer)
                writer.WriteLine()

                writer.WriteBeginTag("table")
                writer.WriteAttribute("style", "width:100%;")
                writer.Write(HtmlTextWriter.TagRightChar)
                writer.WriteFullBeginTag("tr")

                writer.WriteFullBeginTag("td")
                writer.Write(CType(element(0), LocalizableString).GetValue(MyBase.Page.Culture)) 'texte
                writer.WriteEndTag("td")

                writer.WriteBeginTag("td")
                writer.WriteAttribute("style", tdBuiler.ToString)
                writer.Write(HtmlTextWriter.TagRightChar)
                Call Me.RenderInput(writer, CType(element(1), LocalizableString), CType(element(2), Boolean)) 'imput
                writer.WriteEndTag("td")

                writer.WriteEndTag("tr")
                writer.WriteEndTag("table")

                writer.WriteLine()
                writer.WriteEndTag("span")
                writer.WriteLine()
            Next
        End Sub

        Private Sub RenderVerticalAlignRight(ByRef writer As HtmlWriter)
            If String.IsNullOrEmpty(MyBase.StylesSkin.BaseDiv.BaseStyles.Width.ToCss) Then MyBase.StylesSkin.BaseDiv.BaseStyles.Width.SetCss("250px")

            Dim tdBuiler As New StringBuilder()
            tdBuiler.Append(StylesUtils.ConcatCSSValue("text-align:", "right", ";"))

            For Each element As Object In RadioElement.RadioList

                Call RenderVerticalLabel(writer)
                writer.WriteLine()

                writer.WriteBeginTag("table")
                writer.WriteAttribute("style", "width:100%")
                writer.Write(HtmlTextWriter.TagRightChar)
                writer.WriteFullBeginTag("tr")

                writer.WriteFullBeginTag("td")
                Call Me.RenderInput(writer, CType(element(1), LocalizableString), CType(element(2), Boolean)) 'imput
                writer.WriteEndTag("td")

                writer.WriteBeginTag("td")
                writer.WriteAttribute("style", tdBuiler.ToString)
                writer.Write(HtmlTextWriter.TagRightChar)
                writer.Write(CType(element(0), LocalizableString).GetValue(MyBase.Page.Culture)) 'texte
                writer.WriteEndTag("td")

                writer.WriteEndTag("tr")
                writer.WriteEndTag("table")

                writer.WriteLine()
                writer.WriteEndTag("span")
                writer.WriteLine()

            Next
        End Sub

        Private Sub RenderVerticalLabel(ByVal writer As HtmlWriter)
            Dim labelBuilder As New StringBuilder()
            writer.WriteBeginTag("span")
            labelBuilder.Append(StylesUtils.ConcatCSSValue("display:", "block", ";"))
            Select Case Disposition.Alignement
                Case CssEnum.TextAlign.right
                    labelBuilder.Append(StylesUtils.ConcatCSSValue("text-align:", "right", ";"))
                Case CssEnum.TextAlign.center
                    labelBuilder.Append(StylesUtils.ConcatCSSValue("text-align:", "center", ";"))
            End Select
            writer.WriteAttribute("style", labelBuilder.ToString)
            writer.Write(HtmlTextWriter.TagRightChar)
        End Sub

        #End Region 'Methods

        #Region "Other"

        ''Render du label(title) de l'objet
        'Private Sub RenderTitle(ByRef writer As Common.HtmlWriter)
        '    writer.Write(Me.Title.GetHtmlValue(Me, "Title"))
        '    MyBase.RenderEndTextEdit(writer)
        '    writer.WriteLine()
        'End Sub

        #End Region 'Other

    End Class

    <Serializable> _
    Public Class WERadioButtonListDisposition

        #Region "Fields"

        Private _Alignement As CssEnum.TextAlign
        Private _Disposition As RadioButtonDisposition
        Private _LabelsPosition As TextPosition

        #End Region 'Fields

        #Region "Constructors"

        Public Sub New()
            _Alignement = CssEnum.TextAlign.left
            _LabelsPosition = TextPosition.rightmiddle
            _Disposition = RadioButtonDisposition.Vertical
        End Sub

        Public Sub New(ByVal disposing As RadioButtonDisposition, ByVal labelsPos As TextPosition)
            _Disposition = disposing
            _LabelsPosition = labelsPos
        End Sub

        #End Region 'Constructors

        #Region "Properties"

        <Ressource.localizable.LocalizableCatAtt(Ressource.localizable.LocalizableCatAtt.EnumWECategory.Appearance), _
        Ressource.localizable.LocalizableNameAtt("_N114"), _
        LocalizableDescAtt("_D114"), _
        TypeConverter(GetType(TConvCssEnumTextAlign))> _
        Public Property Alignement() As CssEnum.TextAlign
            Get
                Return _Alignement
            End Get
            Set(ByVal value As CssEnum.TextAlign)
                _Alignement = value
            End Set
        End Property

        <Ressource.localizable.LocalizableCatAtt(Ressource.localizable.LocalizableCatAtt.EnumWECategory.Appearance), _
        Ressource.localizable.LocalizableNameAtt("_N112"), _
        LocalizableDescAtt("_D112")> _
        Public Property Disposition() As RadioButtonDisposition
            Get
                Return _Disposition
            End Get
            Set(ByVal value As RadioButtonDisposition)
                _Disposition = value
            End Set
        End Property

        <Ressource.localizable.LocalizableCatAtt(Ressource.localizable.LocalizableCatAtt.EnumWECategory.Appearance), _
        Ressource.localizable.LocalizableNameAtt("_N113"), _
        LocalizableDescAtt("_D113")> _
        Public Property LabelsPosition() As TextPosition
            Get
                Return _LabelsPosition
            End Get
            Set(ByVal value As TextPosition)
                _LabelsPosition = value
            End Set
        End Property

        #End Region 'Properties

        #Region "Methods"

        ''' <summary>
        ''' Compare à l'objet passé en paramètre l'objet courrant. Retourne true si ils sont identiques
        ''' </summary>
        ''' <param name="obj">WERadioButtonListDisposition à comparer</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            Try
                Dim compareObj As WERadioButtonListDisposition = CType(obj, WERadioButtonListDisposition)
                If compareObj.Alignement <> Me.Alignement Then Return False
                If compareObj.Disposition <> Me.Disposition Then Return False
                If compareObj.LabelsPosition <> Me.LabelsPosition Then Return False
            Catch ex As Exception
                Return False
            End Try
            Return True
        End Function

        #End Region 'Methods

    End Class

End Namespace

