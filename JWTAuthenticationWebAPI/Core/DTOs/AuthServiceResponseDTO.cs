namespace JWTAuthenticationWebAPI.Core.DTOs
{
    public class AuthServiceResponseDTO
    {
        #region Constructor
        public AuthServiceResponseDTO() { }

        public AuthServiceResponseDTO(bool isSucceed, string message)
        {
            IsSucceed = isSucceed;
            Message = message;
        }
        #endregion

        public bool IsSucceed { get; set; }
        public string Message { get; set; }
    }
}
