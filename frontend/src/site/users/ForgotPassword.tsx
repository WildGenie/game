import * as React from "react"
import Form from "@/components/ui/forms/Form"
import { useFormInputValidation } from "@/hooks/forms"
import { required } from "@/tools/validations"
import TextField from "@/components/ui/forms/fields/TextField"
import { useState } from "react"
import { forgotPassword } from "@/tools/browser/users"
import { getDocumentTitle } from "@/tools/utils"

const ForgotPassword: React.FunctionComponent = (): JSX.Element => {
	const [wasSuccessful, setWasSuccessful] = useState(false)

	const accountName = useFormInputValidation<string>({
		initialValue: "",
		validators: [required]
	})
	const recaptchaToken = useFormInputValidation<string>({
		initialValue: "",
		validators: [required]
	})

	const onSubmit = async () => {
		const response = await forgotPassword({
			accountName: accountName.value,
			recaptchaToken: recaptchaToken.value
		})

		setWasSuccessful(response.wasSuccessful)

		if (!response.wasSuccessful)
			recaptchaToken.setErrors(response.errors["RecaptchaToken"])

		return wasSuccessful
	}

	document.title = getDocumentTitle("Reset your password")

	return (
		<>
			{!wasSuccessful && (
				<Form
					validators={[accountName.validator, recaptchaToken.validator]}
					handleSubmit={onSubmit}
					recaptchaValue={recaptchaToken.value}
					setRecaptchaValue={recaptchaToken.setValue}
					recaptchaErrors={recaptchaToken.errors}
					recaptchaValidator={recaptchaToken.validator}
					title="Forgot password"
					formClasses="form--standalone"
				>
					<TextField
						labelText="Account name"
						hint="Your username or email address"
						value={accountName.value}
						setValue={accountName.setValue}
						errors={accountName.errors}
						validator={accountName.validator}
						required={true}
					/>
				</Form>
			)}
			{wasSuccessful && (
				<p className="app__message-container">
					If the username or email address you entered is associated with an account, you should receive an email shortly with a link to reset your password.
				</p>
			)}
		</>
	)
}

export default ForgotPassword