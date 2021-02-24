import * as React from "react"
import { Switch, Route } from "react-router-dom"
import { useSelector, useDispatch } from "react-redux"

import "normalize.css"
import "../sass/_main.scss"
import "./App.scss"

import MainMenu from "@/components/nav/MainMenu"
import Footer from "@/components/footer/Footer"
import { getCurrentUser, login } from "@/site/currentUser"
import { checkLogin } from "@/tools/browser/users"
import { useEffect } from "react"
import Home from "@/site/Home"
import Admin from "@/site/admin/Admin"
import Users from "@/site/users/Users"
import MyAccount from "@/site/account/MyAccount"
import FairUse from "@/site/FairUse"

const App = (): JSX.Element => {

	const dispatch = useDispatch()
	const { isLoggedIn }: { isLoggedIn: boolean } = useSelector(getCurrentUser)

	const checkIsLoggedIn = async () => {
		if (isLoggedIn)
			return
		const response = await checkLogin()

		if (response.wasSuccessful && response.result)
			dispatch(login(response.result))
	}

	useEffect(() => {
		if (!isLoggedIn)
			checkIsLoggedIn()
	}, [])

	return (
		<div className="app">
			<MainMenu/>

			<main className="app__body">
				<Switch>
					<Route path="/fair-use">
						<FairUse />
					</Route>
					<Route path="/users">
						<Users />
					</Route>

					<Route path="/admin">
						<Admin />
					</Route>

					<Route path="/account">
						<MyAccount />
					</Route>

					<Route exact path="/">
						<Home />
					</Route>
				</Switch>
			</main>

			<Footer/>
		</div>
	)
}

export default App