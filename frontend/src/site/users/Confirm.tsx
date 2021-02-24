import * as React from "react"
import { useParams, useHistory } from "react-router"
import {Link} from "react-router-dom"
import { Dispatch, SetStateAction, useState, useEffect } from "react"
import FormErrorList from "@/components/ui/forms/FormErrorList"
import { confirm } from "@/tools/browser/users"
import { getDocumentTitle } from "@/tools/utils"

const Confirm: React.FunctionComponent = (): JSX.Element => {
	const { userId, verificationCode } = useParams()
	const history = useHistory()
	const [isConfirmed, setIsConfirmed] = useState(false)
	const [isCompleted, setIsCompleted] = useState(false)
	const [errors, setErrors]: [string[], Dispatch<SetStateAction<string[]>>] = useState([])

	const confirmAccount = async () => {
		const response = await confirm({ userId, verificationCode })
		setIsCompleted(true)
		if (response.wasSuccessful) {
			setIsConfirmed(true)
			setTimeout(() => history.push("/users/login"), 10000)
		} else {
			const responseErrors = []
			if (response.errors) {
				Object.values(response.errors).forEach(err => responseErrors.push(...err))
			}
			setErrors(responseErrors)
		}
	}

	useEffect(() => {
		confirmAccount()
	}, [])

	document.title = getDocumentTitle("Confirm your account")

	return (
		<section className="app__message-container">
			<header>
				<h1>Confirming your account</h1>
			</header>

			{!isConfirmed && !isCompleted && (
				<p>
					We are confirming your account. This may take a few moments.
				</p>
			)}

			{!isConfirmed && isCompleted && (
				<>
					<p>
						There was a problem confirming your account:
					</p>
					<FormErrorList errors={errors}/>
				</>
			)}

			{isConfirmed && (
				<p>
					Your account was confirmed! You should be redirected to the login page momentarily. If not, you can <Link to="/users/login">log in now</Link>.
				</p>
			)}
		</section>
	)
}

export default Confirm