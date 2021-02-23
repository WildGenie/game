import * as React from "react"
import { Switch, Route, useRouteMatch } from "react-router-dom"
import { NavDestination } from "@/tools/definitions/general"
import AdminMenu from "@/components/nav/AdminMenu"
import AddEdit from "@/site/admin/species/AddEdit"
import AddSuccessful from "@/site/admin/species/AddSuccessful"
import Home from "@/site/admin/species/Home"
import Edit from "@/site/admin/species/Edit"
import EditSuccessful from "@/site/admin/species/EditSuccessful"

const links: NavDestination[] = [
	{
		href: "add",
		name: "Add new species"
	}
]

const Species: React.FunctionComponent = (): JSX.Element => {

	const { path } = useRouteMatch()

	return (
		<section className="admin">
			<AdminMenu
				links={links}
				backLink={{ href: "/admin" }}
			/>

			<section className="admin__content">
				<Switch>
					<Route
						exact
						path={path}
					>
						<Home />
					</Route>

					<Route
						exact
						path={`${path}/add`}
					>
						<AddEdit />
					</Route>

					<Route
						exact
						path={`${path}/add/successful`}
					>
						<AddSuccessful />
					</Route>

					<Route path={`${path}/:speciesId/successful`}>
						<EditSuccessful />
					</Route>

					<Route path={`${path}/:speciesId`}>
						<Edit />
					</Route>
				</Switch>
			</section>
		</section>
	)
}

export default Species