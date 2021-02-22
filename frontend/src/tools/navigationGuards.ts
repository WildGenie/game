import { useSelector } from "react-redux"
import { getCurrentUser } from "@/site/currentUser"

/**
 * A navigation guard that requires a user to be logged in.
 */
export function requireLogin(): string {
	const { isLoggedIn }: { isLoggedIn: boolean } = useSelector(getCurrentUser)
	return isLoggedIn ? null : "logged in"
}

/**
 * A navigation guard that requires a user to be a member of the given role.
 *
 * This guard has required arguments. Because of this, requireRole should be passed into Protected.guard with a simple wrapper function.
 *
 * @example
 * <Protected guard={() => requireRole("admin", "an")}/>
 *
 * @param {string} role The role required to access the protected Route
 * @param {"a"|"an"} article The article that should precede the role name in the returned string
 */
export function requireRole(role: string, article: "a" | "an" = "a"): string {
	const { roles }: { roles: string[] } = useSelector(getCurrentUser)
	const inRole = roles && roles.includes(role)
	return inRole ? null : `${article} ${role}`
}