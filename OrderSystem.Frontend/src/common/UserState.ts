import jwtDecode from "jwt-decode";

export enum UserRole {
    Anonym = "Anonym",
    Public = "Public",
    Admin = "Admin"
}

export default class UserState {
    private static accessTokenItemName: string = 'access-token';

    isLogged: boolean;
    accessToken?: string;
    username?: string;
    role: UserRole;

    public static currentState = new UserState();

    constructor() {
        const accessToken = localStorage.getItem(UserState.accessTokenItemName);
        const isAuthenticated = this.isAuthenticated(accessToken);
        if(!isAuthenticated)
            localStorage.removeItem(UserState.accessTokenItemName);

        if(!accessToken || !isAuthenticated) {
            this.isLogged = false;
            this.accessToken = undefined;
            this.username = undefined;
            this.role = UserRole.Anonym;
        }
        else {
            const {username, userRole} = this.getJwtTokenPayload(accessToken);
            this.isLogged = true;
            this.accessToken = accessToken;
            this.username = username;
            this.role = userRole;
        }
    }

    private isAuthenticated(accessToken: any): boolean {
        if(!accessToken) return false;
        const { exp } = jwtDecode(accessToken) as any;
        return Date.now() < exp * 1000;
    }

    private getJwtTokenPayload(accessToken: string) {
        const decodedToken = jwtDecode(accessToken) as any;
        return {
            username: decodedToken.unique_name as string,
            userRole: UserRole[decodedToken.role as keyof typeof UserRole]
        };
    }

    public static login(accessToken: string) {
        localStorage.setItem(UserState.accessTokenItemName, accessToken);
        UserState.currentState = new UserState();
    }

    public static logout() {
        localStorage.removeItem(UserState.accessTokenItemName);
        UserState.currentState = new UserState();
    }
}