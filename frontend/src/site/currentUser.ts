import { Action, ApplicationState } from "@/tools/definitions/general"
import { ApplicationUserModel } from "@/tools/definitions/users"

export const LOGIN = "LOGIN"
export const LOGOUT = "LOGOUT"

export type CurrentUser = {
	username: string
	email: string
	isLoggedIn: boolean
	isVerified: boolean
	roles: string[]
}

export function login(userData: ApplicationUserModel): Action<CurrentUser> {
	return {
		type: LOGIN,
		payload: {
			username: userData.userName,
			email: userData.email,
			isVerified: userData.isVerified,
			roles: userData.roles,
			isLoggedIn: true
		}
	}
}

export function logout(): Action<CurrentUser> {
	return {
		type: LOGOUT,
		payload: {
			username: "",
			email: "",
			isVerified: false,
			roles: [],
			isLoggedIn: false
		}
	}
}

export function currentUserReducer(currentlyLoggedInUser: CurrentUser = {
	username: "",
	email: "",
	isVerified: false,
	roles: [],
	isLoggedIn: false
}, action: Action<CurrentUser>): CurrentUser {
	if (action.type === LOGIN || action.type === LOGOUT)
		return action.payload

	return currentlyLoggedInUser
}

export function getCurrentUser(state: ApplicationState): CurrentUser {
	return state.currentUser
}