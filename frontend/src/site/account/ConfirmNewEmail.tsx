import * as React from "react"
import { useParams } from "react-router"
import { Dispatch, SetStateAction, useState, useEffect } from "react"
import FormErrorList from "@/components/ui/forms/FormErrorList"
import { performEmailChange } from "@/tools/browser/users"
import { getDocumentTitle } from "@/tools/utils"

const Confirm: React.FunctionComponent = (): JSX.Element => {
	const { userId, verificationCode, newEmail } = useParams()
	const [isConfirmed, setIsConfirmed] = useState(false)
	const [isCompleted, setIsCompleted] = useState(false)
	const [errors, setErrors]: [string[], Dispatch<SetStateAction<string[]>>] = useState([])

	const confirmNewEmail = async () => {
		const response = await performEmailChange({ userId, verificationCode, newEmail })
		setIsCompleted(true)
		if (response.wasSuccessful) {
			setIsConfirmed(true)
		} else {
			setErrors(response.errors["Unknown"])
		}
	}

	useEffect(() => {
		confirmNewEmail()
	}, [])

	document.title = getDocumentTitle("Confirm your new email address")

	return (
		<section className="app__message-container">
			<header>
				<h1>Confirming your new email</h1>
			</header>

			{!isConfirmed && !isCompleted && (
				<p>
					We are confirming your account. This may take a few moments.
				</p>
			)}

			{!isConfirmed && isCompleted && (
				<>
					<p>
						There was a problem confirming your new email:
					</p>
					<FormErrorList errors={errors}/>
				</>
			)}

			{isConfirmed && (
				<p>
					Your new email address was confirmed!
				</p>
			)}
		</section>
	)
}

export default Confirm