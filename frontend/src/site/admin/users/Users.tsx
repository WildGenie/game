import * as React from "react"
import { Dispatch, SetStateAction, useEffect, useState } from "react"
import { Route, Switch, useParams, useRouteMatch } from "react-router-dom"
import { ApplicationUserModel } from "@/tools/definitions/users"
import { getUserData } from "@/tools/browser/admin"
import Home from "@/site/admin/users/Home"
import Email from "@/site/admin/users/Email"
import AdminMenu from "@/components/nav/AdminMenu"
import { NavDestination } from "@/tools/definitions/general"
import Password from "@/site/admin/users/Password"
import Roles from "@/site/admin/users/Roles"

const Users: React.FunctionComponent = (): JSX.Element => {

	const { path } = useRouteMatch()

	const links: NavDestination[] = [
		{
			href: "email",
			name: "Email"
		},
		{
			href: "password",
			name: "Password"
		},
		{
			href: "roles",
			name: "Roles"
		}
	]

	const [user, setUser]: [ApplicationUserModel, Dispatch<SetStateAction<ApplicationUserModel>>] = useState(null)
	const { userId } = useParams()

	const getUser = async () => {
		const response = await getUserData(userId)
		if (response.wasSuccessful)
			setUser(response.result)
	}
	useEffect(() => {
		getUser()
	}, [])

	return (
		<section className="admin">
			<AdminMenu
				links={links}
				backLink={{ href: "/admin" }}
			/>

			<section className="admin__content">
				<Switch>
					<Route
						path={path}
						exact
					>
						<Home user={user} />
					</Route>

					<Route path={`${path}/email`}>
						<Email user={user} />
					</Route>

					<Route path={`${path}/password`}>
						<Password user={user} />
					</Route>

					<Route path={`${path}/roles`}>
						{user && (
							<Roles user={user} />
						) || (
							<p>Loading...</p>
						)}
					</Route>
				</Switch>
			</section>
		</section>
	)
}

export default Users