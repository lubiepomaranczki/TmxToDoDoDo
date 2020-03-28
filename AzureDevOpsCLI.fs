module AzureDevOpsCLI

open System.Net
open System.IO
open System
open Newtonsoft.Json

type TaskProperty =
    { op : string
      path : string
      from : string
      value : string }

let Base64Encode(plainText : string) : string =
    let plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText)
    Convert.ToBase64String(plainTextBytes)

let postDocRaw (url : string, data : string) : string =
    let data' : byte [] = System.Text.Encoding.ASCII.GetBytes(data)

    let token = Base64Encode("{email}:{password}")
    let autorization = String.Format("Basic {0}", token)

    let request = WebRequest.Create(url)
    request.Method <- "POST"
    request.ContentType <- "application/json-patch+json"

    request.PreAuthenticate <- true
    request.Headers.Add("Authorization", autorization)

    use wstream = request.GetRequestStream()
    wstream.Write(data', 0, (data'.Length))
    wstream.Flush()
    wstream.Close()

    let response = request.GetResponse()
    use reader = new StreamReader(response.GetResponseStream())
    let output = reader.ReadToEnd()

    reader.Close()
    response.Close()
    request.Abort()

    output

let getRequestBodyForAddTask (line : string, fileInfo : FileInfo) =
    let newLineMarker = "&nbsp;"
    let descripion =
        String.Format
            ("<div>In File: <b>{0}</b></div> {1} <div>Please take care of this comment: {2}</div> <pre><code><div>{3}</div></code></pre> {4} <div>From F# with Love <3</div> ",
             fileInfo.Name, newLineMarker, newLineMarker, line, newLineMarker)
    let title = String.Format("TODO comment in {0}", fileInfo.Name)

    let request =
        [ { op = "add"
            path = "/fields/System.Title"
            from = null
            value = title }
          { op = "add"
            path = "/fields/System.Tags"
            from = null
            value = "auto-added, technical debt" }
          { op = "add"
            path = "/fields/System.Description"
            from = null
            value = descripion } ]
    JsonConvert.SerializeObject(request)