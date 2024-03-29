﻿#Region "Header"

' // Video HTML5 element (Dmit) /////////////////////////////////////////////////////
'Imports openElement.Tools

#End Region 'Header

Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Web
Imports System.Web.UI

Imports openElement.Tools
Imports openElement.WebElement.Common
Imports openElement.WebElement.Common.Attributes
Imports openElement.WebElement.Editors
Imports openElement.WebElement.Editors.Converter
Imports openElement.WebElement.Elements
Imports openElement.WebElement.HtAccess
Imports openElement.WebElement.LinksManager
Imports openElement.WebElement.StylesManager

Imports WebElement.Elements.Media.Editors
Imports WebElement.Elements.Media.Editors.Converter
Imports WebElement.My.Resources.text
Imports WebElement.Ressource.localizable

Imports LocalizableCatAtt = WebElement.Ressource.localizable.LocalizableCatAtt

Imports LocalizableNameAtt = WebElement.Ressource.localizable.LocalizableNameAtt

Imports Page = openElement.WebElement.Page

Namespace Elements.Media

    <Serializable> _
    Public Class WEVideo
        Inherits ElementBase

        #Region "Fields"

        Public Shared ArFormatNames As String() = {"video/mp4", "video/webm", "video/ogg"}

        Private _CustomControls As Boolean = False 'custom controls that replace browser controls
        Private _EnableControls As Boolean = True 'browser default controls
        Private _ExportValues As String = ""
        Private _Links As List(Of Link) 'obsolete, for deserialization of older projects until/including 1.32R7
        Private _Links2 As Dictionary(Of String, Link) 'les liens dans une ordre fixe selon format

        ' ReSharper disable VBWarnings::BC40008
        Private _NoPluginMsg As CodeBlock 'obsolete codeblock
        Private _NoPluginMsgV2 As ScriptBlock
        Private _PosterImage As Link
        Private _SWFPlayerLink As Link
        Private _VAutoPlay As Boolean = False
        Private _VLoop As Boolean = False
        Private _VMute As Boolean = False

        #End Region 'Fields

        #Region "Constructors"

        Public Sub New(ByVal page As Page, ByVal parentID As String, ByVal templateName As String)
            MyBase.New(EnuElementType.PageEdit, "WEVideoHTML5", page, parentID, templateName)
        End Sub

        #End Region 'Constructors

        #Region "Enumerations"

        ' openElement.DB.DBElem.WEDynamic
        ' pour ajouter le nouveau format: changer ArFormatNames aussi!
        Public Enum EnuVideoTypes
            Unknown = -1
            MP4 = 0
            WEBM = 1 ' ce format n'est pas necessaire je crois
            OGG = 2
        End Enum

        #End Region 'Enumerations

        #Region "Properties"

        '<Ressource.localizable.LocalizableCatAtt(Ressource.localizable.LocalizableCatAtt.EnumWECategory.Behavior), _
        'Ressource.localizable.LocalizableNameAtt("_N186"), _
        'Ressource.localizable.LocalizableDescAtt("_D186"), _
        'Common.Attributes.PageUpdateMode(Common.Attributes.PageUpdateMode.EnuUpdateMode.ElementWithCss)> _
        <Browsable(False)> _
        Public Property CustomControls() As Boolean
            ' alternative customized controls
            Get
                Return _CustomControls
            End Get
            Set(ByVal value As Boolean)
                _CustomControls = value
            End Set
        End Property

        <LocalizableCatAtt(LocalizableCatAtt.EnumWECategory.Behavior), _
        LocalizableNameAtt("_N185"), _
        LocalizableDescAtt("_D185"), _
        PageUpdateMode(PageUpdateMode.EnuUpdateMode.ElementWithCss)> _
        Public Property EnableControls() As Boolean
            ' browser default controls
            Get
                Return _EnableControls
            End Get
            Set(ByVal value As Boolean)
                _EnableControls = value
            End Set
        End Property

        <Browsable(False), _
        ExportVar(ExportVar.EnuVarType.Js)> _
        Public Property ExportValues() As String
            ' to export localised text etc. to JS
            Get
                If String.IsNullOrEmpty(_ExportValues) Then
                    _ExportValues = LocalizableOpen._0292 + "##" + LocalizableOpen._0294 'TODO
                End If
                Return _ExportValues
            End Get
            Set(ByVal value As String)
                _ExportValues = value
            End Set
        End Property

        <Browsable(False), _
        ExportVar(ExportVar.EnuVarType.Js)> _
        Public ReadOnly Property Links() As List(Of Link)
            ' for compatibility with js code, to export video links to JS
            Get
                ' create an ordered list, with Nothing in place of missing videos:
                Dim l As New List(Of Link)
                For i = 0 To UBound(ArFormatNames)
                    Dim tp As String = ArFormatNames(i)
                    If VideoLinks.ContainsKey(tp) AndAlso VideoLinks(tp) IsNot Nothing Then
                        l.Add(VideoLinks(tp))
                    Else
                        l.Add(Nothing)
                    End If
                Next
                Return l
            End Get
        End Property

        '<Ressource.localizable.LocalizableCatAtt(Ressource.localizable.LocalizableCatAtt.EnumWECategory.Edition), _
        'Ressource.localizable.LocalizableNameAtt("_N190"), _
        'Ressource.localizable.LocalizableDescAtt("_D190"), _
        'Editor(GetType(openElement.WebElement.Editors.UITypeCodeBlock), GetType(Drawing.Design.UITypeEditor)), _
        'TypeConverter(GetType(Editors.Converter.TConvCodeVideoNoPlugin)), _
        'Common.Attributes.PageUpdateMode(Common.Attributes.PageUpdateMode.EnuUpdateMode.Page), _
        'Common.Attributes.ConfigBiblio(True, False, False, False, False)> _
        'Public Property NoPluginMsg() As openElement.WebElement.DataType.CodeBlock
        '    Get
        '        If _NoPluginMsg Is Nothing Then
        '            _NoPluginMsg = New openElement.WebElement.DataType.CodeBlock
        '        End If
        '        Dim r As String = _NoPluginMsg.Code.GetValue(MyBase.Page.Culture)
        '        If String.IsNullOrEmpty(r) Then
        '            r = My.Resources.text.LocalizableFormAndConverter._0142 + "<br/>" + _
        '                "<a href='http://get.adobe.com/flashplayer/' target='_blank'><u style='color:#FF0000;'><b>Flash</b> (" + My.Resources.text.LocalizableFormAndConverter._0143 + ")</u></a>" + ControlChars.NewLine + _
        '                "<span class='VidHTML5NotIE' data-note='This span will be hidden in IE'> " + My.Resources.text.LocalizableFormAndConverter._0144 + " " + ControlChars.NewLine + _
        '                "<a href='http://www.apple.com/quicktime/download/' target='_blank'><u style='color:#FF0000;'><b>QuickTime</b> (" + My.Resources.text.LocalizableFormAndConverter._0143 + ")</u></a></span>"
        '            _NoPluginMsg.Code.SetValue(r, MyBase.Page.Culture)
        '        End If
        '        Return _NoPluginMsg
        '    End Get
        '    Set(ByVal value As openElement.WebElement.DataType.CodeBlock)
        '        If _NoPluginMsg Is Nothing Then
        '            _NoPluginMsg = New openElement.WebElement.DataType.CodeBlock
        '        End If
        '        _NoPluginMsg = value
        '    End Set
        'End Property
        <LocalizableCatAtt(LocalizableCatAtt.EnumWECategory.Edition), _
        LocalizableNameAtt("_N190"), _
        LocalizableDescAtt("_D190"), _
        Editor(GetType(UITypeScriptBlock), GetType(UITypeEditor)), _
        TypeConverter(GetType(TConvCodeVideoNoPlugin)), _
        PageUpdateMode(PageUpdateMode.EnuUpdateMode.Page), _
        ConfigBiblio(True, False, False, False, False)> _
        Public Property NoPluginMsgV2() As ScriptBlock
            Get
                If _NoPluginMsgV2 Is Nothing Then
                    _NoPluginMsgV2 = New ScriptBlock(Me.ID, EnuScriptType.Html)
                    If _NoPluginMsg IsNot Nothing Then
                        _NoPluginMsgV2.Code = _NoPluginMsg.Code
                        _NoPluginMsgV2.Type = _NoPluginMsg.ScriptType
                    Else
                        Dim r As String = LocalizableFormAndConverter._0142 + "<br/>" + _
                         "<a href='http://get.adobe.com/flashplayer/' target='_blank'><u style='color:#FF0000;'><b>Flash</b> (" + LocalizableFormAndConverter._0143 + ")</u></a>" + ControlChars.NewLine + _
                         "<span class='VidHTML5NotIE' data-note='This span will be hidden in IE'> " + LocalizableFormAndConverter._0144 + " " + ControlChars.NewLine + _
                         "<a href='http://www.apple.com/quicktime/download/' target='_blank'><u style='color:#FF0000;'><b>QuickTime</b> (" + LocalizableFormAndConverter._0143 + ")</u></a></span>"
                        _NoPluginMsgV2.Code.SetValue(r, MyBase.Page.Culture)
                    End If
                End If
                Return _NoPluginMsgV2
            End Get
            Set(ByVal value As ScriptBlock)
                _NoPluginMsgV2 = value
            End Set
        End Property

        <LocalizableCatAtt(LocalizableCatAtt.EnumWECategory.Edition), _
        LocalizableNameAtt("_N184"), _
        LocalizableDescAtt("_D184"), _
        Editor(GetType(UITypeLinkFile), GetType(UITypeEditor)), _
        TypeConverter(GetType(TConvLinkFile)), _
        PageUpdateMode(PageUpdateMode.EnuUpdateMode.Page), _
        ConfigBiblio(True, False, False, False, False)> _
        Public Property PosterImage() As Link
            Get
                If _PosterImage Is Nothing Then _PosterImage = New Link
                Return _PosterImage
            End Get
            Set(ByVal value As Link)
                _PosterImage = value
            End Set
        End Property

        <Browsable(False)> _
        Public Property SWFPlayerLink() As Link
            Get
                If _SWFPlayerLink Is Nothing Then _SWFPlayerLink = New Link
                Return _SWFPlayerLink
            End Get
            Set(ByVal value As Link)
                _SWFPlayerLink = value
            End Set
        End Property

        <LocalizableCatAtt(LocalizableCatAtt.EnumWECategory.Behavior), _
        LocalizableNameAtt("_N187"), _
        LocalizableDescAtt("_D187"), _
        PageUpdateMode(PageUpdateMode.EnuUpdateMode.ElementWithCss)> _
        Public Property VAutoPlay() As Boolean
            Get
                Return _VAutoPlay
            End Get
            Set(ByVal value As Boolean)
                _VAutoPlay = value
            End Set
        End Property

        ' Common.Attributes.ExportVar(Common.Attributes.ExportVar.EnuVarType.Js), _
        <LocalizableCatAtt(LocalizableCatAtt.EnumWECategory.Edition), _
        LocalizableNameAtt("_N183"), _
        LocalizableDescAtt("_D183"), _
        Editor(GetType(UITypeVideo), GetType(UITypeEditor)), _
        PageUpdateMode(PageUpdateMode.EnuUpdateMode.Page)> _
        Public Property VideoLinks() As Dictionary(Of String, Link)
            Get
                If _Links2 Is Nothing Then _Links2 = New Dictionary(Of String, Link)
                If _Links IsNot Nothing AndAlso _Links2.Count < 1 Then
                    For i As Integer = 0 To _Links.Count - 1
                        Dim link = _Links(i) : If link Is Nothing Then Continue For
                        _Links2(ArFormatNames(i)) = link
                    Next
                End If
                _Links = Nothing
                Return _Links2
            End Get
            Set(ByVal value As Dictionary(Of String, Link))
                _Links2 = value
                _Links = Nothing
            End Set
        End Property

        <LocalizableCatAtt(LocalizableCatAtt.EnumWECategory.Behavior), _
        LocalizableNameAtt("_N188"), _
        LocalizableDescAtt("_D188"), _
        PageUpdateMode(PageUpdateMode.EnuUpdateMode.ElementWithCss)> _
        Public Property VLoop() As Boolean
            Get
                Return _VLoop
            End Get
            Set(ByVal value As Boolean)
                _VLoop = value
            End Set
        End Property

        'TODO DD res et descriptions
        <LocalizableCatAtt(LocalizableCatAtt.EnumWECategory.Behavior), _
        LocalizableNameAtt("_N189"), _
        LocalizableDescAtt("_D189"), _
        PageUpdateMode(PageUpdateMode.EnuUpdateMode.ElementWithCss)> _
        Public Property VMute() As Boolean
            Get
                Return _VMute
            End Get
            Set(ByVal value As Boolean)
                _VMute = value
            End Set
        End Property

        #End Region 'Properties

        #Region "Methods"

        '' ==================================================================================================
        Public Function GetLinkForVideoFormatInd(ByVal formatIndex As EnuVideoTypes) As Link
            If formatIndex > UBound(ArFormatNames) OrElse formatIndex < 0 Then Return Nothing
            Dim tp As String = ArFormatNames(formatIndex)
            If Not VideoLinks.ContainsKey(tp) Then Return Nothing Else Return VideoLinks(tp)
        End Function

        Protected Overrides Function OnGetInfo() As ElementInfo
            Dim info As New ElementInfo(Me)
            info.ToolBoxCaption = LocalizableOpen._0287 ' "Vidéo HTML5"
            info.VersionMajor = 1
            info.VersionMinor = 0
            info.GroupName = "NBGroupMedia"
            info.ToolBoxIco = My.Resources.WEVideo
            info.ToolBoxDescription = LocalizableOpen._0288 '"Ajouter une vidéo HTML5."
            info.AutoOpenProperty = "VideoLinks"
            info.SortPropertyList.Add(New SortProperty("VideoLinks", "movie.png", LocalizableOpen._0058)) '"Sélection du l'URL"
            Return info
        End Function

        Protected Overrides Sub OnInitExternalFiles()
            MyBase.AddExternalScripts(EnuScriptType.Javascript, "ElementsLibrary\Common\Client\WEVideo.js", "WEFiles/Client/WEVideo.js")
            SWFPlayerLink = MyBase.AddExternalScripts(EnuScriptType.OtherScript, "ElementsLibrary\Common\Client\gddflvplayer.swf", "WEFiles/Client/gddflvplayer.swf")
            MyBase.OnInitExternalFiles()
        End Sub

        Protected Overrides Sub OnOpen()
            Dim configStylesZones As New List(Of ConfigStylesZone)
            ' ConfigStylesZones.Add(New StylesManager.ConfigStylesZone("StyleVidHTML5", My.Resources.text.LocalizableOpen._0159, My.Resources.text.LocalizableOpen._0160)) ' TODO DD Names

            MyBase.OnOpen(configStylesZones)
        End Sub

        'Private Sub __writeParamTag(ByVal writer As HtmlWriter, ByVal name As String, ByVal value As String)
        '    writer.WriteBeginTag("param")
        '    writer.WriteAttribute("name", name)
        '    writer.WriteAttribute("value", value)
        '    writer.Write(HtmlTextWriter.SelfClosingTagEnd)
        '    writer.WriteLine()
        'End Sub
        '' (not used at the moment) QuickTime video object
        'Private Sub _writeQTReplacement(ByVal writer As HtmlWriter)
        '    If Not __IsMP4Present() Then Exit Sub
        '    Dim mp4_link As String = MyBase.GetLink(GetLinkForVideoFormatInd(EnuVideoTypes.MP4)).Replace(" ", "%20")
        '    'Taille de l'élément
        '    Dim ElmWidth As Integer = MyBase.StylesSkin.BaseDiv.BaseStyles.Width.GetNumber
        '    If ElmWidth = 0 Then ElmWidth = MyBase.StylesSkin.StylesSkinModel.BaseDiv.BaseStyles.Width.GetNumber
        '    Dim ElmHeight As Integer = MyBase.StylesSkin.BaseDiv.BaseStyles.Height.GetNumber
        '    If ElmHeight = 0 Then ElmHeight = MyBase.StylesSkin.StylesSkinModel.BaseDiv.BaseStyles.Height.GetNumber
        '    Dim PlayerWidth As String = (ElmWidth).ToString
        '    Dim PlayerHeight As String = (ElmHeight).ToString '  - 16 ' 16 is control bar's constant height as stated by Apple
        '    If MyBase.Page.RenderMode = Page.EnuTypeRenderMode.Editor Then
        '        PlayerWidth = PlayerWidth '* 0.64
        '        PlayerHeight = PlayerHeight - 32
        '        writer.Write("<font style=""font-size:10px;background-color:white;color:red;"">" + LocalizableOpen._0293 + "</font>") ' DD translate
        '    End If
        '    '<object classid="clsid:02BF25D5-8C17-4B23-BC80-D3488ABDDC6B" codebase="http://www.apple.com/qtactivex/qtplugin.cab" height="100%" width="100%">
        '    writer.WriteLine()
        '    writer.WriteBeginTag("object")
        '    writer.WriteAttribute("classid", "clsid:02BF25D5-8C17-4B23-BC80-D3488ABDDC6B")
        '    writer.WriteAttribute("codebase", "http://www.apple.com/qtactivex/qtplugin.cab")
        '    writer.WriteAttribute("width", PlayerWidth)
        '    writer.WriteAttribute("height", PlayerHeight)
        '    writer.Write(HtmlTextWriter.TagRightChar)
        '    writer.WriteLine()
        '    '<param name="scale" value="aspect" />
        '    '<param name="width" value="100%" />
        '    '<param name="height" value="100%" />
        '    '<param name="controller" value="true" />
        '    '<param name="autoplay" value="false" />
        '    '<param name="src" value="'+linkToUse+'" />
        '    __writeParamTag(writer, "scale", "aspect")
        '    __writeParamTag(writer, "width", PlayerWidth)
        '    __writeParamTag(writer, "height", PlayerHeight)
        '    __writeParamTag(writer, "controller", "true")
        '    __writeParamTag(writer, "autoplay", "false")
        '    __writeParamTag(writer, "src", mp4_link)
        '    '<embed type="video/quicktime"
        '    '     scale="aspect"
        '    '     pluginspage="http://www.apple.com/quicktime/download/"
        '    '     controller="true"
        '    '     autoplay="false"
        '    '     src=""
        '    '     height="100%"
        '    '     width="100%" />
        '    writer.WriteBeginTag("embed")
        '    writer.WriteAttribute("type", "video/quicktime")
        '    writer.WriteAttribute("scale", "aspect")
        '    writer.WriteAttribute("pluginspage", "http://www.apple.com/quicktime/download/")
        '    writer.WriteAttribute("controller", "true")
        '    writer.WriteAttribute("autoplay", "false")
        '    writer.WriteAttribute("src", mp4_link)
        '    writer.WriteAttribute("width", PlayerWidth)
        '    writer.WriteAttribute("height", PlayerHeight)
        '    writer.Write(HtmlTextWriter.SelfClosingTagEnd)
        '    writer.WriteLine()
        '    writer.WriteEndTag("object")
        'End Sub
        Protected Overrides Sub Render(ByVal writer As HtmlWriter)
            MyBase.RenderBeginTag(writer)

            ' ////////////////////////////////////////////////////////
            ' http://www.robwalshonline.com/posts/tutorial-serving-html5-video-cross-browser-including-ipad/
            ' http://blog.noinc.com/2010/05/13/html5-video-tag-iphone-ipad-ihaveheadache/
            ' MyBase.StylesSkin.FindStylesZone("Test").FindStyles(StylesManager.StylesZone.EnuStyleState.Over).Width.SetCss("16px")

            If __IsMP4Present() Or __IsOGGPresent() Or __IsWEBMPresent() Or _
               MyBase.Page.RenderMode <> Page.EnuTypeRenderMode.Editor Then '  Or MyBase.Page.RenderMode = openElement.WebElement.Page.EnuTypeRenderMode.Editor

                Call _WriteVideoHTML5Tag(writer)
                ' Call _WriteVideoHTML5Tag(writer, True) ' with added <noscript> code

                ' write video types into .htaccess file
                Dim htaccessCommands As String = _
                    "AddType video/ogg .ogg" + Environment.NewLine + _
                    "AddType video/ogg .ogv" + Environment.NewLine + _
                    "AddType video/mp4 .mp4" + Environment.NewLine + _
                    "AddType video/mp4 .m4v" + Environment.NewLine + _
                    "AddType video/mp4 .mov" + Environment.NewLine + _
                    "AddType video/webm .webm" + Environment.NewLine
                'Dim htcommand As New HtCdeFreeScript(htaccessCommands)
                Various.AddHtAccessCommand(New HtCdeFreeScript(htaccessCommands))

            Else
                'Affichage si aucun lien & dans l'editeur
                Call _WriteFailCode(writer)
            End If

            ' ////////////////////////////////////////////////////////

            MyBase.RenderEndTag(writer)
        End Sub

        ' code html pour l'editeur ou l'echec
        Private Sub _WriteFailCode(ByVal writer As HtmlWriter)
            writer.WriteBeginTag("table")
            writer.WriteAttribute("style", "width:100%; height:100%;") ' background-color:#999999;

            'Dim styleVid As StylesManager.Styles = MyBase.StylesSkin.FindStylesZone("StyleVidHTML5").FindStyles(StylesManager.StylesZone.EnuStyleState.Normal)
            'styleVid.Width.SetCss("100%")
            'styleVid.Height.SetCss("100%")

            'styleVid.Background.ImageLink = PosterImage
            'styleVid.Background.Position.Predefined = StylesManager.CssItems.CssEnum.Position.center_center
            'styleVid.Background.Repeat = StylesManager.CssItems.CssEnum.Repeat.no_repeat

            'If MyBase.Page.RenderMode = openElement.WebElement.Page.EnuTypeRenderMode.Editor Then writer.WriteAttribute("style", "background-image:url(""" + MyBase.GetLink(PosterImage) + """);")
            'writer.WriteAttribute("class", MyBase.GetStyleZoneClass("StyleVidHTML5"))

            writer.Write(HtmlTextWriter.TagRightChar)
            writer.WriteLine()

            writer.WriteFullBeginTag("tr")
            writer.WriteBeginTag("td")
            writer.WriteAttribute("style", "text-align:center; vertical-align:middle; background-color: black; color: #FF8800; font-size:10px;")
            writer.Write(HtmlTextWriter.TagRightChar)
            writer.WriteLine()

            '_writeQTReplacement(writer) ' QuickTime fallback (removed when necessary in WEVideo.js)
            If __IsMP4Present() Then
                writer.WriteBeginTag("div")
                writer.WriteAttribute("class", "NoPluginMsgVideoHTML5")
                writer.WriteAttribute("style", "width:100%; height:100%; display:none;")
                writer.Write(HtmlTextWriter.TagRightChar) ' ">"
                writer.WriteLine()
                writer.Write(NoPluginMsgV2.Code.GetValue(MyBase.Page.Culture))
                writer.WriteEndTag("div")
            Else ' no fallback possible:
                writer.Write(LocalizableFormAndConverter._0145) ' No compatible video source

                '!! Test code, remove
                '  ____________________________________________________
                ' // Test dynamic mode /////////////////////////////////
                'If WEDynamic.DBFunctionalityActivated Or IsDynamic Then
                '    ' <div> dynamic: <div tytle="Image_Name">Image_Name: url(Image_URL)</div>
                '    DTWriteBeginTag("div", writer)               ' <div
                '    DTWriteAttrDynamic("title", "Image.Title")   ' title="My image"
                '    DTWriteAttrDynamic("style", "LoginReport", "color:{IF Condition_LogFail}red{ELSE}black{ENDIF};", "color:#888888;") ' style="color:black/red/lightgrey;"
                '    DTTagEndDeclaration()                        ' > (also writes identification class)
                '    DTWriteInnerHtmlDynamic("Image.Title,Image.URL", _
                '                              "{P0}: link is <a href=""{P1}"">{P1}</a>", "No data") ' My image: url(www.site.com/image.jpg)
                '    ' "Image" is the property created in DynInitialize()
                '    writer.WriteEndTag("div")                              ' </div>
                'End If '!!

            End If
            writer.WriteLine()

            'writer.WriteBeginTag("img")
            'writer.WriteAttribute("src", MyBase.GetLink(PosterImage)) ' TODO DD Changer image
            'writer.WriteAttribute("alt", "")
            'writer.Write(System.Web.UI.HtmlTextWriter.SelfClosingTagEnd)

            writer.WriteEndTag("td")
            writer.WriteEndTag("tr")
            writer.WriteEndTag("table")
            writer.WriteLine()
        End Sub

        ' main function: write <video> tag with all parameters and QuickTime last-chance fallback code; tag will later be modified by WEVideo.js
        Private Sub _WriteVideoHTML5Tag(ByVal writer As HtmlWriter, Optional ByVal noJSmode As Boolean = False)
            ' <div class="ContainerVideoHTML5" style="width:100%; height:100%;">

            writer.WriteBeginTag("div")
            writer.WriteAttribute("class", "ContainerVideoHTML5")
            writer.WriteAttribute("style", "width:100%; height:100%;")
            writer.Write(HtmlTextWriter.TagRightChar) ' ">"
            writer.WriteLine()

            ' <video width="400" height="222" controls="controls">
            writer.WriteBeginTag("video")

            ' video element style:
            'Dim styleVid As StylesManager.Styles = MyBase.StylesSkin.FindStylesZone("StyleVidHTML5").FindStyles(StylesManager.StylesZone.EnuStyleState.Normal)
            'styleVid.Width.SetCss("100%") : styleVid.Height.SetCss("100%")

            writer.WriteAttribute("preload", "none")

            writer.WriteAttribute("class", "TagVideoHTML5")
            writer.WriteAttribute("style", "width:100%; height:100%;")

            ' for IE9 (and maybe Safari), write MP4 source -> so that it doesn't need to reset it -> which would lead to losing poster image (ha ha)
            If __IsMP4Present() Then
                Dim srcMP4 As String = MyBase.GetLink(GetLinkForVideoFormatInd(EnuVideoTypes.MP4))
                srcMP4 = HttpUtility.UrlPathEncode(srcMP4)
                writer.WriteAttribute("src", srcMP4)
            End If
            Dim browserControls As Boolean = (EnableControls And Not CustomControls)
            If browserControls Then writer.WriteAttribute("controls", "controls")
            If CustomControls Then writer.WriteAttribute("custcontrols", "true")

            If VAutoPlay Then writer.WriteAttribute("autoplay", "autoplay")
            If VLoop Then writer.WriteAttribute("loop", "loop")
            If VMute Then writer.WriteAttribute("muted", "muted")

            ' poster/affiche (image d'init)
            Dim posterLink As String = ""
            If PosterImage IsNot Nothing Then posterLink = MyBase.GetLink(PosterImage) ' PosterImage.GetSitePath(Me.Page.Culture)
            If Not String.IsNullOrEmpty(posterLink) Then writer.WriteAttribute("poster", posterLink)

            ' lien vers lecteur swf pour Flash Fallback (no need if no mp4 source)
            If __IsMP4Present() Then
                Dim swfLink As String = ""
                If SWFPlayerLink IsNot Nothing Then swfLink = MyBase.GetLink(SWFPlayerLink) ' _SWFPlayerLink.GetSitePath(Me.Page.Culture)
                swfLink = Various.GetDebugPathWhenUsed(SWFPlayerLink, swfLink)
                If Not String.IsNullOrEmpty(swfLink) Then writer.WriteAttribute("data-swfplayer", swfLink)
            End If

            writer.Write(HtmlTextWriter.TagRightChar) ' ">"
            writer.WriteLine()

            ' used ONLY for no JavaScript mode: writing <source> subtags with alternative links (otherwise disabled for iPad etc. compatibility)
            If (noJSmode) Then
                writer.WriteFullBeginTag("noscript")
                Dim index As Integer = -1
                For Each kvp In VideoLinks
                    index += 1
                    Dim link = kvp.Value : If link Is Nothing Then Continue For
                    __WriteVideoFormat(writer, link, kvp.Key)
                Next
                writer.WriteEndTag("noscript")
            End If

            'Ici l'alternative en cas d'echec:
            Call _WriteFailCode(writer)

            writer.WriteEndTag("video") ' </video>

            '!! Test code, remove
            '  ____________________________________________________
            ' // Test dynamic mode /////////////////////////////////
            'If WEDynamic.DBFunctionalityActivated Or IsDynamic Then
            '    ' <div> dynamic: <div tytle="Image_Name">Image_Name: url(Image_URL)</div>
            '    DTWriteBeginTag("div", writer)               ' <div
            '    DTWriteAttrDynamic("title", "Image.Title")              ' title="My image"
            '    DTWriteAttrDynamic("style", "LoginReport", "color:{IF Condition_LogFail}red{ELSE}black{ENDIF};", "color:#888888;") ' style="color:black/red/lightgrey;"
            '    DTTagEndDeclaration()                              ' > (also writes identification class)
            '    DTWriteInnerHtmlDynamic("Image.Title,Image.URL", _
            '                              "{P0}: link is <a href=""{P1}"">{P1}</a>", "No data") ' My image: url(www.site.com/image.jpg)
            '    ' "Image" is the property created in DynInitialize()
            '    writer.WriteEndTag("div")                              ' </div>
            'End If '!!

            writer.WriteEndTag("div") ' </div>
        End Sub

        Private Function __IsMP4Present() As Boolean
            Return (GetLinkForVideoFormatInd(EnuVideoTypes.MP4) IsNot Nothing) AndAlso Not String.IsNullOrEmpty(GetLinkForVideoFormatInd(EnuVideoTypes.MP4).GetSitePath(Me.Page.Culture))
        End Function

        '' ex. "video/mp4" pour MP4 video
        'Private Function __GetFormatTypeDescription(ByVal index As Integer) As String
        '    If index < 0 Or index >= ArFormatNames.Length Or Not System.Enum.IsDefined(GetType(EnuVideoTypes), index) Then Return ""
        '    Return ArFormatNames(index)
        'End Function
        Private Function __IsOGGPresent() As Boolean
            Return (GetLinkForVideoFormatInd(EnuVideoTypes.OGG) IsNot Nothing) AndAlso Not String.IsNullOrEmpty(GetLinkForVideoFormatInd(EnuVideoTypes.OGG).GetSitePath(Me.Page.Culture))
        End Function

        Private Function __IsWEBMPresent() As Boolean
            Return (GetLinkForVideoFormatInd(EnuVideoTypes.WEBM) IsNot Nothing) AndAlso Not String.IsNullOrEmpty(GetLinkForVideoFormatInd(EnuVideoTypes.WEBM).GetSitePath(Me.Page.Culture))
        End Function

        ' not used now
        Private Sub __WriteVideoFormat(ByVal writer As HtmlWriter, ByVal link As Link, ByVal type As String)
            If Not (String.IsNullOrEmpty(link.GetSitePath(Me.Page.Culture))) AndAlso Not (String.IsNullOrEmpty(type)) Then
                '  <source src="video.mp4" type="video/mp4" />
                writer.WriteBeginTag("source")
                writer.WriteAttribute("src", MyBase.GetLink(link))
                writer.WriteAttribute("type", type)
                writer.Write(HtmlTextWriter.SelfClosingTagEnd)
                writer.WriteLine()
            End If
        End Sub

        #End Region 'Methods

        #Region "Other"

        'Protected Overrides Sub DynInitialize()
        'End Sub

        #End Region 'Other

    End Class

End Namespace

