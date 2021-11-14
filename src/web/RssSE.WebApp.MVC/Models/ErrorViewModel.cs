namespace RssSE.WebApp.MVC.Models
{
    public class ErrorViewModel
    {
        public int ErrorCode { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }

        public ErrorViewModel(int errorCode, string title, string message)
        {
            ErrorCode = errorCode;
            Title = title;
            Message = message;
        }

        public ErrorViewModel()
        {

        }

        public static class ErroViewModelFactory
        {
            public static ErrorViewModel CreateErrorViewModel(int statusCode)
            {
                return statusCode switch
                {
                    500 => new ErrorViewModel(statusCode, "Ocorreu um erro!"
                                               , "Ocorreu um erro! Tente novamente mais tarde ou contate nosso suporte"),
                    400 => new ErrorViewModel(statusCode, "Ops! Página não encontrada.",
                                                "A página que está procurando não existe! <br/> Em caso de dúvidas entre em contato com nosso suporte"),
                    403 => new ErrorViewModel(statusCode, "Acesso negado",
                                                "Você não tem permissão para fazer isto"),
                    _ => new ErrorViewModel(),
                };
            }
        }
    }
}
