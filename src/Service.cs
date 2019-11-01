using System;
using awscsharp.Tv24EpgGenerator;

namespace awscsharp
{
    public class Service
    {
        private readonly ITv24EpgGenerator _tv24EpgGenerator;

        public Service(ITv24EpgGenerator tv24EpgGenerator)
        {
            _tv24EpgGenerator = tv24EpgGenerator;
        }

        public void Run()
        {
            try
            {
                _tv24EpgGenerator.GenerateTVProgram().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}