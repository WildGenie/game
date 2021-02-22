import * as React from "react"
import { Route, Switch, useRouteMatch } from "react-router-dom"

import Protected from "@/components/Protected"
import { requireLogin } from "@/tools/navigationGuards"
import { NavDestination } from "@/tools/definitions/general"
import AdminMenu from "@/components/nav/AdminMenu"
import ConfirmNewEmail from "@/site/account/ConfirmNewEmail"
import AccountEmail from "@/site/account/AccountEmail"
import AccountPassword from "@/site/account/AccountPassword"
import PersonalData from "@/site/account/PersonalData"
import AccountHome from "@/site/account/AccountHome"

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
		href: "personal-data",
		name: "Personal data"
	}
]

const MyAccount: React.FunctionComponent = (): JSX.Element => {

	const { url } = useRouteMatch()

	return (
		<Protected guard={requireLogin}>
			<section className="app__widget app__widget--lg app__widget--parent admin">
				<AdminMenu links={links}/>

				<section className="app__widget app__widget--md admin__content">
					<Switch>
						<Route
							exact
							path={url}
						>
							<AccountHome />
						</Route>

						<Route
							exact
							path={`${url}/email`}
						>
							<AccountEmail />
						</Route>

						<Route path={`${url}/email/:newEmail/:userId/:verificationCode`}>
							<ConfirmNewEmail />
						</Route>

						<Route path={`${url}/password`}>
							<AccountPassword />
						</Route>

						<Route path={`${url}/personal-data`}>
							<PersonalData />
						</Route>
					</Switch>
				</section>
			</section>
		</Protected>
	)
}

export default MyAccount