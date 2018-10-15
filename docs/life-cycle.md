(ASP.NET Application Life Cycle Overview for IIS 7.0)[https://msdn.microsoft.com/en-us/library/bb470252.aspx]

```
Validate the request, which examines the information sent by the browser and determines whether it contains potentially malicious markup. For more information, see ValidateRequest and Script Exploits Overview.
Perform URL mapping, if any URLs have been configured in the UrlMappingsSection section of the Web.config file.
Raise the BeginRequest event.
Raise the AuthenticateRequest event.
Raise the PostAuthenticateRequest event.
Raise the AuthorizeRequest event.
Raise the PostAuthorizeRequest event.
Raise the ResolveRequestCache event.
Raise the PostResolveRequestCache event.
Raise the MapRequestHandler event. An appropriate handler is selected based on the file-name extension of the requested resource. The handler can be a native-code module such as the IIS 7.0 StaticFileModule or a managed-code module such as the PageHandlerFactory class (which handles .aspx files). 
Raise the PostMapRequestHandler event.
Raise the AcquireRequestState event.
Raise the PostAcquireRequestState event.
Raise the PreRequestHandlerExecute event.
Call the ProcessRequest method (or the asynchronous version IHttpAsyncHandler.BeginProcessRequest) of the appropriate IHttpHandler class for the request. For example, if the request is for a page, the current page instance handles the request.
Raise the PostRequestHandlerExecute event.
Raise the ReleaseRequestState event.
Raise the PostReleaseRequestState event.
Perform response filtering if the Filter property is defined.
Raise the UpdateRequestCache event.
Raise the PostUpdateRequestCache event.
Raise the LogRequest event.
Raise the PostLogRequest event.
Raise the EndRequest event.
Raise the PreSendRequestHeaders event.
Raise the PreSendRequestContent event.
```

> Don't use the last too - breaks IIS + async.