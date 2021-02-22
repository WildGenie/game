import { CurrentUser } from "@/site/currentUser"

// Server-side account roles
export enum Roles {
	administrator = "Administrator"
}

/**
 * An interface describing a NavigationGuard function
 *
 * A NavigationGuard returns null if navigation passes, or a string describing the failing requirement. The string must grammatically follow the phrase "You do not have permission to do that. You must be " {...}
 *
 * @example
 * // passes navigation
 * someNavigationGuard() => null
 * @example
 * // fails navigation
 * someNavigationGuard() => "logged in" // must be logged in
 * @example
 * // fails navigation
 * someNavigationGuard() => "an administrator" // must be an administrator
 */
export interface NavigationGuard {
	(): string
}

// Redux Action definition
export type Action<TPayload> = {
	type: string,
	payload: TPayload
}

// Redux state definition
export type ApplicationState = {
	currentUser: CurrentUser
}

export type NavDestination = {
	href: string,
	name?: string,
	icon?: string
}