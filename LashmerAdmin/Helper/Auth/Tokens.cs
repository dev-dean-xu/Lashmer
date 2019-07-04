using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LashmerAdmin.Models;
using Newtonsoft.Json;

namespace LashmerAdmin.Helper.Auth
{
	public class Tokens
	{
		public static async Task<string> GenerateJwt(ClaimsIdentity identity, IJwtFactory jwtFactory, string userName,
			string userRole, JwtIssuerOptions jwtOptions, JsonSerializerSettings serializerSettings)
		{
			var response = new
			{
				user_id = identity.Claims.Single(c => c.Type == "id").Value,
				user_name = userName,
				user_role = userRole,
				auth_token = await jwtFactory.GenerateEncodedToken(userName, identity).ConfigureAwait(false),
				expires_in = (int) jwtOptions.ValidFor.TotalSeconds
			};

			return JsonConvert.SerializeObject(response, serializerSettings);
		}
	}
}
