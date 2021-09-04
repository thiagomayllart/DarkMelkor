# DarkMelkor

DarkMelkor is a modified version of Melkor, from @b33f (FuzzySecurity): https://github.com/FuzzySecurity/Sharp-Suite.
Melkor was originally released as a tool able to load .NET assemblies in disposable AppDomains, keeping them encrypted in memory while they are not being invoked.
This came up as an alternative to fork&run tasks since it would be possible to load, invoke and discard the AppDomains in the same process, instead of loading the CLR in a sacrificial process and waiting for it to finish execution.
Unfortunately, the original project was not able to reference the loaded assembly in the disposable AppDomain in case you are loading it in a injected process, due to calling it from a “no context” assembly.

While searching for an alternative to the mentioned problem, this article: https://www.accenture.com/us-en/blogs/cyber-defense/clrvoyance-loading-managed-code-into-unmanaged-processes from Bryan Alexander and Josh Stone came up with an interesting solution. It is possible to create two CrossAppDomainDelegates: one of them referencing a function that can be resolved by our "no context" assembly (basically anything in the mscorlib) and the other being our malicious function. After that we can patch the initial bytes of the first function with the adress of the malicious one, in a way that when calling the non-malicious one, it will endup jumping to the address of the second function.

Credit goes to these folks: @b33f, Bryan Alexander and Josh Stone. I've just assembled these ideas with small modifications.
