namespace JerryPlat.Models
{
    public enum AnswerType
    {
        //判断题
        Judgement = 1,

        //单选题
        Single = 2,

        //多选题
        Mutiple = 3
    }

    public enum Sex
    {
        //男
        Male = 1,

        //女
        Famale = 0
    }

    public enum WxTicketType
    {
        ACCESS_TOKEN = 1,
        TICKET = 2
    }
}