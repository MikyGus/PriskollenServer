# PriskollenServer

- [PriskollenServer](#priskollenserver)
  - [Logging](#logging)


## Logging
Edit **ServerUrl** in ```appsettings.json``` to be directed to your seq server.
Start server with:

```bash
docker run -d --restart unless-stopped --name Seq -e ACCEPT_EULA=y -v D:\seq:/data -p 8081:80 datalust/seq:latest
```