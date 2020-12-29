[![Github license](mit.svg)](https://github.com/engineering87/SharpTracer/blob/master/LICENSE)

# SharpTracer
SharpTracer is an experimental project for distributed tracing in microservices architectures based on gRPC services. SharpTracer is a central gRPC service for the historicization of the executions of each gRPC service present in the reference architecture.
Project is currently being tested and many developments are still work in progress.

### How it works
SharpTracer exposes two methods for the historicization of the traces and the orderly display of the history for each service.
Each gRPC service present within the microservices architecture will have to inform the SharpTracer central service, which will take care of ordering the events for each gRPC service based on the local timestamp. The ordered history of each individual gRPC service will be displayed on the **HistoryAsync** method.
Currently the historicization is only in memory.

### How to use it
To use the SharpTracer service it is necessary to deploy and integrate it within the reference architecture. Any other gRPC service will have to refer to the **tracer.proto** file, invoking the **TraceAsync** at each communication made to the other gRPC services, explaining the source and destination of the message.

### Contributing
Thank you for considering to help out with the source code!
If you'd like to contribute, please fork, fix, commit and send a pull request for the maintainers to review and merge into the main code base.
Theoretical advice on how to improve distributed tracing logic is also welcome.

**Getting started with Git and GitHub**

 * [Setting up Git for Windows and connecting to GitHub](http://help.github.com/win-set-up-git/)
 * [Forking a GitHub repository](http://help.github.com/fork-a-repo/)
 * [The simple guide to GIT guide](http://rogerdudler.github.com/git-guide/)
 * [Open an issue](https://github.com/engineering87/SharpTracer/issues) if you encounter a bug or have a suggestion for improvements/features

### Licensee
SharpTracer source code is available under MIT License, see license in the source.

### Contact
Please contact at francesco.delre.87[at]gmail.com for any details.
