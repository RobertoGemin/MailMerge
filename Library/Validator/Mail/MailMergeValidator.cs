using Library.Interface.Mail;
using Library.Interface.Validator;
using Library.Validator.BaseClass;
 using Library.Validator.Processing;

namespace Library.Validator.Mail
{
    public class MailMergeValidator : Message
    {
        private readonly IMailMerge _mailMerge;
        private readonly IValidator _directoryCreationValidator;
        private readonly IValidator _fileValidator;
        private readonly IValidator _templateValidator;
        private readonly IValidator _xmlValidator;
        private string _templateFile;
        private string _xmlFile;
        private string _outputDir;

        public static MailMergeValidator CreateMailMergeValidator()
        {
            return new MailMergeValidator(
                directoryCreationValidator: new DirectoryCreationValidator(),
                fileValidator: new FileValidator(),
                templateValidator: new TemplateValidator(),
                xmlValidator: new XmlValidator(),
                mailMerge: new MailMerge()
            );
        }
        private MailMergeValidator(IValidator directoryCreationValidator, IValidator fileValidator, IValidator templateValidator, IValidator xmlValidator, IMailMerge mailMerge)
        {
            _templateFile = string.Empty;
            _xmlFile = string.Empty;
            _outputDir = string.Empty;

            _directoryCreationValidator = directoryCreationValidator;
            _fileValidator = fileValidator;
            _templateValidator = templateValidator;
            _xmlValidator = xmlValidator;
            _mailMerge = mailMerge;
            _mailMerge.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(_mailMerge.Messages))
                {
                    Messages = _mailMerge.Messages;
                }
            };
        }

        public bool Validate(string templateFile, string xmlFile, string outputDir)
        {
            _templateFile = templateFile;
            _xmlFile = xmlFile;
            _outputDir = outputDir;

            return ValidateWith(_fileValidator, templateFile)
                   && ValidateWith(_fileValidator, xmlFile)
                   && ValidateWith(_directoryCreationValidator, outputDir)
                   && ValidateWith(_templateValidator, templateFile)
                   && ValidateWith(_xmlValidator, xmlFile);
        }

        private bool ValidateWith(IValidator validator, string input)
        {
            if (validator.Validate(input))
            {
                Messages = validator.Message;
                return true;
            }
            else
            {
                Messages = validator.Message;
                return false;
            }
        }


        public override void Process()
        {
            _mailMerge.MergeMail(_templateFile, _xmlFile, _outputDir);
        }

    }
}
