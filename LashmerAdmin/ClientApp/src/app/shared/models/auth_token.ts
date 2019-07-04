export class Auth_Token {
	//user_id: string;
	//user_name: string;
	//user_role: string;
	//auth_token: string;
	//expires_in: number;

	constructor(public user_id: string,
		public user_name: string,
		public user_role: string,
		public auth_token: string,
		public expires_in: number) {
	}
}

