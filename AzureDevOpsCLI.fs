module AzureDevOpsCLI

open System.Net
open System.IO
open System

let Base64Encode(plainText : string) : string =
    let plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText)
    Convert.ToBase64String(plainTextBytes)

let postDocRaw (url : string, data : string) : string =
    let data' : byte [] = System.Text.Encoding.ASCII.GetBytes(data)

    let token = Base64Encode("{email}:{password}")
    let autorization = String.Format("Basic {0}", token)
    printf "Token: %s" autorization

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