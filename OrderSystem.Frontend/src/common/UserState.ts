import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import jwtDecode from "jwt-decode";

export enum UserRole {
    Anonym = "Anonym",
    Public = "Public",
    Admin = "Admin"
}

export class UserState {
    private static accessTokenItemName: string = 'access-token';

    isLogged: boolean;
    accessToken?: string;
    username?: string;
    role: UserRole;

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

    isAuthenticated(accessToken: any): boolean {
        if(!accessToken) return false;
        const { exp } = jwtDecode(accessToken) as any;
        return Date.now() < exp * 1000;
    }

    getJwtTokenPayload(accessToken: string) {
        const decodedToken = jwtDecode(accessToken) as any;
        console.log(decodedToken);
        return {
            username: decodedToken.username as string,
            userRole: UserRole[decodedToken.role as keyof typeof UserRole]
        };
    }
}

const initialUserState = new UserState();

const userSlice = createSlice({
	name: "user",
	initialState: initialUserState,
	reducers: {
        login(state: UserState, action: PayloadAction<string>): UserState {
			localStorage.setItem('access-token', action.payload);
			return new UserState();
		},
		logout(state: UserState) : UserState {
			localStorage.removeItem('access-token');
			return new UserState();
		}
	},
});

export const {login, logout } = userSlice.actions

export default userSlice.reducer