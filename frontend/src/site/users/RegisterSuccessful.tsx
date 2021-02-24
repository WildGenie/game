import * as React from "react"
import { useEffect, useState } from "react"
import { useHistory } from "react-router"
import { getDocumentTitle } from "@/tools/utils"

const RegisterSuccessful: React.FunctionComponent = (): JSX.Element => {
	const history = useHistory()
	const [countdown, setCountdown] = useState(10)

	const updater: VoidFunction = (): void => {
		if (countdown === 1)
			history.push("/")
		else
			setCountdown(countdown - 1)
	}

	useEffect(() => {
		const timeoutId = setTimeout(updater, 1000)
		return () => clearTimeout(timeoutId)
	}, [countdown])

	document.title = getDocumentTitle("Registration successful")

	return (
		<section className="app__message-container">
			<header>
				<h1>Registration successful</h1>
			</header>

			<p>
				Your account was created successfully. You should receive an email shortly with a link to confirm your account.
			</p>
			<p>
				If you try to log in before confirming your account, our system will generate a new confirmation email and the old one will be invalidated.
			</p>
			<p>
				You will be redirected to the homepage in {countdown} seconds.
			</p>
		</section>
	)
}

export default RegisterSuccessful