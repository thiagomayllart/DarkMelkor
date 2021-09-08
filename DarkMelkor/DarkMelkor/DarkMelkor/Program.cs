using System;
using System.IO;
using System.Threading;

namespace DarkMelkor
{
    class Program
    {
        public static void runTest(string run)
        {
            // Encrypt module
            //==============
            Console.WriteLine("[>] Reading assembly as Byte[]");
            Byte[] bMod = File.ReadAllBytes(@"C:\Users\Thiago Mayllart\Downloads\Sharp-Suite-master2\Sharp-Suite-master\Melkor\demoModule\demoModule\bin\Debug\demoModule.exe");//change it wth the path of the compiled demoModule
            Console.WriteLine("[>] DPAPI CryptProtectData -> assembly[]");
            DarkMelkor.DPAPI_MODULE dpMod = DarkMelkor.dpapiEncryptModule(bMod, "Melkor", 0);
            if (dpMod.pMod != IntPtr.Zero)
            {
                Console.WriteLine("    |_ Success");
                Console.WriteLine("    |_ pCrypto : 0x" + String.Format("{0:X}", (dpMod.pMod).ToInt64()));
                Console.WriteLine("    |_ iSize   : " + dpMod.iModSize);
                bMod = null;
            } else
            {
                Console.WriteLine("\n[!] Failed to DPAPI encrypt module..");
                return;
            }

            Console.WriteLine("\n[?] Press enter to continue..");

            // Create AppDomain & load module
            //==============
            Console.WriteLine("[>] DPAPI CryptUnprotectData -> assembly[] copy");
            DarkMelkor.DPAPI_MODULE oMod = DarkMelkor.dpapiDecryptModule(dpMod);
            if (oMod.iModSize != 0)
            {
                Console.WriteLine("    |_ Success");
            } else
            {
                Console.WriteLine("\n[!] Failed to DPAPI decrypt module..");
                return;
            }
            Console.WriteLine("[>] Create new AppDomain and invoke module through proxy..");
            AppDomain oAngband = null;
            try
            {
                oAngband = DarkMelkor.loadAppDomainModule("dothething", "Angband", oMod.bMod, run);
            }
            catch (Exception ex)
            {
            }

            Console.WriteLine("\n[?] Press enter to continue..");

            // Remove Appdomain and free CryptUnprotectData
            //==============
            Console.WriteLine("[>] Unloading AppDomain");
            DarkMelkor.unloadAppDomain(oAngband);
            Console.WriteLine("[>] Freeing CryptUnprotectData");
            DarkMelkor.freeMod(oMod);

            Console.WriteLine("\n[?] Press enter to exit..");
        }

        static void Main(string[] args)
        {
            Thread thread = new Thread(() => runTest("first"));
            Thread thread2 = new Thread(() => runTest("second"));
            thread.Start();
            thread2.Start();
        }
    }
}
