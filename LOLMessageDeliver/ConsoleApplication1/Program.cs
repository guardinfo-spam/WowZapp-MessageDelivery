using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LOLMessageDelivery;
using LOLCodeLibrary.LoggingSystem;

namespace ConsoleApplication1
{
    class Program
    {

        private static void ShowMenu()
        {
            Console.WriteLine("Which tests do you want to run ?");
            Console.WriteLine("0 - All");
            Console.WriteLine("1 - MessageCreate");
            Console.WriteLine("2 - MessageMarkRead");
            Console.WriteLine("3 - MessageGetNew");
            Console.WriteLine("4 - MessageStepDataSave");


            Console.WriteLine("X - Exit");

            int testID = 0;
            var read = Console.ReadLine();
            if (!read.ToLower().Equals("x"))
            {
                int.TryParse(read, out testID);
                RunBattery(testID);
            }
        }

        private static void RunBattery(int id)
        {
            ILogger logger = new LoggerManager(new ConsoleLogger()).GetManager();
            LOLConnect2.LOLMessageClient ws = new LOLConnect2.LOLMessageClient();

            switch (id)
            {
                case 0:
                    Test_MessageCreate testMessageCreate = new Test_MessageCreate(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for MessageCreate", true);
                    testMessageCreate.RunTests();
                    logger.LogMessage("================================", true);

                    Test_MessageMarkRead testMessageMarkRead = new Test_MessageMarkRead(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for MessageMarkRead", true);
                    testMessageMarkRead.RunTests();
                    logger.LogMessage("================================", true);

                    Test_MessageGetNew testMessageGetNew = new Test_MessageGetNew(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for MessageGetNew", true);
                    testMessageGetNew.RunTests();
                    logger.LogMessage("================================", true);
                    
                    Test_MessageStepDataSave testMessageStepDataSave = new Test_MessageStepDataSave(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for MessageStepDataSave", true);
                    testMessageStepDataSave.RunTests();
                    logger.LogMessage("================================", true);

                    
                    break;
                case 1:
                    Test_MessageCreate testMessageCreate1 = new Test_MessageCreate(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for MessageCreate", true);
                    testMessageCreate1.RunTests();
                    logger.LogMessage("================================", true);
                    break;
                case 2:
                    Test_MessageMarkRead testMessageMarkRead1 = new Test_MessageMarkRead(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for MessageMarkRead", true);
                    testMessageMarkRead1.RunTests();
                    logger.LogMessage("================================", true);
                    break;
                case 3:
                    Test_MessageGetNew testMessageGetNew1 = new Test_MessageGetNew(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for MessageGetNew", true);
                    testMessageGetNew1.RunTests();
                    logger.LogMessage("================================", true);
                    break;
                case 4:
                    Test_MessageStepDataSave testMessageStepDataSave1 = new Test_MessageStepDataSave(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for MessageStepDataSave", true);
                    testMessageStepDataSave1.RunTests();
                    logger.LogMessage("================================", true);
                    break;
                default:
                    logger.LogMessage("Please pass a valid option ( 0-4 )", true);
                    break;
            }
        }

        static void Main(string[] args)
        {
            LOLConnect2.LOLMessageClient ws = new LOLConnect2.LOLMessageClient();
            ShowMenu();

            //LOLConnect2.LOLMessageClient ws = new LOLConnect2.LOLMessageClient();
            //LOLConnect2.MessageStep tmpStep = new LOLConnect2.MessageStep();


            //Guid authenticationToken = ws.AuthenticationTokenGet("1234567890");

            //List<LOLConnect2.MessageStep> steps = new List<LOLConnect2.MessageStep>();

            //tmpStep.MessageText = "This is a test message";
            //tmpStep.StepNumber = 1;
            //tmpStep.StepType = LOLConnect2.MessageStep.StepTypes.Text;
            
            //steps.Add(tmpStep);

            //tmpStep = new LOLConnect2.MessageStep();
            //tmpStep.StepNumber = 2;
            //tmpStep.StepType = LOLConnect2.MessageStep.StepTypes.Comix;
            //tmpStep.ContentPackItemID = 6;

            //steps.Add(tmpStep);

            //tmpStep = new LOLConnect2.MessageStep();
            //tmpStep.StepNumber = 3;
            //tmpStep.StepType = LOLConnect2.MessageStep.StepTypes.Polling;
            
            //steps.Add(tmpStep);



            //LOLConnect2.Message tmpMessage = new LOLConnect2.Message();

            //tmpMessage.FromAccountID = new Guid("12345678-1234-1234-1234-123456789012");  // My accounts guid (this is fake here)

            //List<Guid> toAccounts = new List<Guid>();
            //toAccounts.Add(System.Guid.NewGuid()); // Fake Guid for who we are sending to.

            //tmpMessage = ws.MessageCreate(tmpMessage, steps, toAccounts, authenticationToken);

            //LOLConnect2.PollingStep pollingStep = new LOLConnect2.PollingStep();
            //pollingStep.PollingAnswer1 = "Test Answer 1";
            //pollingStep.PollingAnswer2 = "Test Answer 2";
            //pollingStep.MessageID = tmpMessage.MessageID;
            //pollingStep.PollingQuestion = "Question";
            //ws.PollingStepSave(pollingStep, authenticationToken);


            ////This retrieves messages sent to you from someone specific
            //List<LOLConnect2.Message> Messages = ws.MessageGetFrom(toAccounts[0], new Guid("12345678-1234-1234-1234-123456789012"), new DateTime(1900, 01, 01), new DateTime(2099, 01, 01), 50, new List<Guid>(), authenticationToken);

            ////This gets new messages for your device
            //ws.MessageGetNew(toAccounts[0], "1234567890", new List<Guid>(), authenticationToken);

            ////This gets a list of the steps that are in a message
            //ws.MessageGetSteps(Messages[0].MessageID, authenticationToken);


            ////This marks a message as read so it doesn't continue to show as new.
            //ws.MessageMarkRead(Messages[0].MessageID, toAccounts[0], "1234567890", authenticationToken);

        }
    }
}
