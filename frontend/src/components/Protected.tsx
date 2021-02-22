import * as React from "react"
import { useState, useEffect } from "react"
import { useHistory } from "react-router-dom"
import { NavigationGuard } from "@/tools/definitions/general"

type ProtectedProps = {
	guard: NavigationGuard
	children: React.ReactNode
}

const Protected: React.FunctionComponent<ProtectedProps> = ({ guard, children }: ProtectedProps): JSX.Element => {

	const errorMessage = guard()

	const [countdown, setCountdown] = useState(5)
	const history = useHistory()

	useEffect(() => {
		if (errorMessage !== "logged in")
			return
		const timeoutId = setTimeout(() => {
			if (countdown === 0)
				history.push("/users/login")
			setCountdown(countdown - 1)
		}, 1000)
		return () => clearTimeout(timeoutId)
	}, [countdown, errorMessage])

	return (
		<>
			{!errorMessage ? children : (
				<p className="app__message-container">
					You do not have permission to do that. You must be {errorMessage}
				</p>
			)}
		</>
	)
}

export default Protected