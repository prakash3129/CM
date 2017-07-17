// <copyright file="WhiteCalculatorTest.cs" >
//   Copyright @ Md. jawed. All rights reserved.
// </copyright>
// <summary>
//  Demo project to Automate Calculator using White Framework.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using White.Core.Factory;
using White.Core.UIItems.Finders;

namespace TestWhiteCalculator
{    
    class WhiteCalculatorTest
    {        
        private static White.Core.Application _application;        
        private static White.Core.UIItems.WindowItems.Window _mainWindow;        
        static void Main(string[] args)
        {
            try
            {                                
                _application = White.Core.Application.Attach(Process.GetProcessesByName("iSystems 3.0")[0].Id);                
                _mainWindow = _application.GetWindow(SearchCriteria.ByText("iSystem Panel"), InitializeOption.NoCache);
                List<White.Core.UIItems.UIItem> itm = new List<White.Core.UIItems.UIItem>();
                White.Core.UIItems.UIItemCollection xxx = _mainWindow.Items;

                foreach (White.Core.UIItems.UIItem i in xxx)
                {
                    if(i.ToString().StartsWith("Panel"))
                    {                        
                        foreach(White.Core.UIItems.UIItem Ucol in ((White.Core.UIItems.Panel)i).Items)
                        {                            
                            itm.Add(Ucol);
                        }                        
                    }
                    else if (i.ToString().StartsWith("GroupBox"))
                    {
                        foreach (White.Core.UIItems.UIItem Ucol in ((White.Core.UIItems.GroupBox)i).Items)
                        {
                            itm.Add(Ucol);
                        }
                    }

                }


                string s = ((White.Core.UIItems.ListBoxItems.Win32ComboBox)_mainWindow.Items[6]).SelectedItemText;
            }
            catch (Exception ex)
            {                
                throw;
            }

        }        
    }
}
