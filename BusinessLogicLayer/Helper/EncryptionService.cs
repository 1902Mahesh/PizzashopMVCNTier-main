using Microsoft.AspNetCore.Cryptography.KeyDerivation;

public class EncryptionService
{
    public string EncryptPassword(string password){
        if(string.IsNullOrEmpty(password)){
            return null;
        }
       string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
           password: password,
           salt: new byte[0],
           prf: KeyDerivationPrf.HMACSHA1,
           iterationCount: 10000,
           numBytesRequested: 256 / 8));
        return hashed;
    }

}
