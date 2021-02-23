import * as React from "react"
import { Route, Switch, useRouteMatch } from "react-router-dom"

import Protected from "@/components/Protected"
import { requireRole } from "@/tools/navigationGuards"
import { Roles } from "@/tools/definitions/general"
import Home from "@/site/admin/Home"
import UsersList from "@/site/admin/users/UsersList"
import Users from "@/site/admin/users/Users"
import Species from "@/site/admin/species/Species"

const Admin: React.FunctionComponent = (): JSX.Element => {

	const { path } = useRouteMatch()

	return (
		<Protected guard={() => requireRole(Roles.administrator, "an")}>
			<Switch>
				<Route
					path={path}
					exact
				>
					<Home/>
				</Route>
				<Route
					exact
					path={`${path}/users`}
				>
					<UsersList/>
				</Route>
				<Route path={`${path}/users/:userId`}>
					<Users/>
				</Route>
				<Route path={`${path}/species`}>
					<Species />
				</Route>
			</Switch>
		</Protected>
	)
}

export default Admin