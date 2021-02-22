import * as React from "react"
import { Route, Switch, useRouteMatch } from "react-router-dom"
import Register from "@/site/users/Register"
import RegisterSuccessful from "@/site/users/RegisterSuccessful"
import Confirm from "@/site/users/Confirm"
import Login from "@/site/users/Login"
import Logout from "@/site/users/Logout"
import ForgotPassword from "@/site/users/ForgotPassword"
import ResetPassword from "@/site/users/ResetPassword"

const Users: React.FunctionComponent = (): JSX.Element => {

	const { path } = useRouteMatch()

	return (
		<Switch>
			<Route path={`${path}/register`}>
				<Register/>
			</Route>

			<Route path={`${path}/registration-successful`}>
				<RegisterSuccessful/>
			</Route>

			<Route path={`${path}/confirm/:userId/:verificationCode`}>
				<Confirm/>
			</Route>

			<Route path={`${path}/login`}>
				<Login/>
			</Route>

			<Route path={`${path}/logout`}>
				<Logout/>
			</Route>

			<Route path={`${path}/forgot-password`}>
				<ForgotPassword/>
			</Route>

			<Route path={`${path}/reset-password/:userId/:verificationCode`}>
				<ResetPassword/>
			</Route>
		</Switch>
	)
}

export default Users