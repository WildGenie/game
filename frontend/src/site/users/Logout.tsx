import * as React from "react"
import { useState, useEffect } from "react"
import { useDispatch } from "react-redux"
import { useHistory } from "react-router-dom"
import { logout } from "@/tools/browser/users"
import { logout as doLogout } from "@/site/currentUser"
import { getDocumentTitle } from "@/tools/utils"

const Logout: React.FunctionComponent = (): JSX.Element => {

	const dispatch = useDispatch()

	useEffect(() => {
		logout()
		dispatch(doLogout())
	}, [])

	const [countdown, setCountdown] = useState(10)
	const history = useHistory()

	useEffect(() => {
		if (countdown < 1)
			history.push("/")

		const timeoutId = setTimeout(() => setCountdown(countdown - 1), 1000)

		return () => clearTimeout(timeoutId)
	}, [countdown])

	document.title = getDocumentTitle("Log out")

	return (
		<section>
			<p className="app__message-container">
				You have been logged out successfully. You will be redirected to the homepage in {countdown} seconds.
			</p>
		</section>
	)
}

export default Logout