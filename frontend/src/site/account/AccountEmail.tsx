import * as React from "react"
import { useState } from "react"

import Form from "@/components/ui/forms/Form"
import { OnSubmitValidator } from "@/tools/definitions/forms"
import { useFormInputValidation } from "@/hooks/forms"
import { required, requireMatch } from "@/tools/validations"
import TextField from "@/components/ui/forms/fields/TextField"
import { initiateEmailChange } from "@/tools/browser/users"
import { getDocumentTitle } from "@/tools/utils"

const AccountEmail: React.FunctionComponent = (): JSX.Element => {

	const [wasSuccessful, setWasSuccessful] = useState(false)

	const email = useFormInputValidation<string>({
		initialValue: "",
		validators: [required]
	})
	const confirmEmail = useFormInputValidation<string>({
		initialValue: ""
	})
	const confirmPassword = useFormInputValidation<string>({
		initialValue: "",
		validators: [required]
	})

	confirmEmail.validators.push(() => requireMatch(email.value, confirmEmail.value, "The email addresses do not match"))

	const onSubmit: OnSubmitValidator = async () => {
		const response = await initiateEmailChange({
			email: email.value,
			confirmEmail: confirmEmail.value,
			confirmPassword: confirmPassword.value
		})

		if (!response.wasSuccessful) {
			email.setErrors(response.errors["Email"])
			confirmEmail.setErrors(response.errors["ConfirmEmail"])
			confirmPassword.setErrors(response.errors["ConfirmPassword"])
		}

		setWasSuccessful(response.wasSuccessful)

		return response.wasSuccessful
	}

	document.title = getDocumentTitle("Update your email address")

	return (
		<>
			{!wasSuccessful && (
				<Form
					validators={[email.validator, confirmEmail.validator, confirmPassword.validator]}
					handleSubmit={onSubmit}
					title="Account email"
					showReset={false}
					showRecaptcha={false}
				>
					<TextField
						labelText="New email address"
						required={true}
						value={email.value}
						setValue={email.setValue}
						errors={email.errors}
						validator={email.validator}
					/>
					<TextField
						labelText="Confirm new email address"
						required={true}
						value={confirmEmail.value}
						setValue={confirmEmail.setValue}
						errors={confirmEmail.errors}
						validator={confirmEmail.validator}
					/>
					<TextField
						labelText="Confirm your password"
						type="password"
						required={true}
						value={confirmPassword.value}
						setValue={confirmPassword.setValue}
						errors={confirmPassword.errors}
						validator={confirmPassword.validator}
					/>
				</Form>
			)}
			{wasSuccessful && (
				<p>
					Please check your new email address for a link to confirm your new email address. Your email address will not be updated until you confirm.
				</p>
			)}
		</>
	)
}

export default AccountEmail