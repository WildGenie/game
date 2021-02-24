import * as React from "react"
import { Dispatch, SetStateAction, useState } from "react"
import { useParams, Link } from "react-router-dom"
import Form from "@/components/ui/forms/Form"
import { OnSubmitValidator } from "@/tools/definitions/forms"
import { useFormInputValidation } from "@/hooks/forms"
import { required, requireDigit, requireLength, requireLowerCase, requireMatch, requireSpecialChar, requireUpperCase } from "@/tools/validations"
import TextField from "@/components/ui/forms/fields/TextField"
import { resetPassword } from "@/tools/browser/users"
import { ResetPasswordModelErrorKeys } from "@/tools/definitions/users"
import { getDocumentTitle } from "@/tools/utils"

const ResetPassword: React.FunctionComponent = (): JSX.Element => {

	const newPassword = useFormInputValidation<string>({
		initialValue: "",
		validators: [required, requireLowerCase, requireUpperCase, requireDigit, requireSpecialChar, (input: string) => requireLength(input, 8, 64)]
	})
	const confirmNewPassword = useFormInputValidation<string>({
		initialValue: ""
	})
	confirmNewPassword.validators.push(() => requireMatch(newPassword.value, confirmNewPassword.value, "The passwords do not match"))

	const [wasSuccessful, setWasSuccessful] = useState(false)
	const [errors, setErrors]: [string[], Dispatch<SetStateAction<string[]>>] = useState([])
	const {
		userId,
		verificationCode
	}: Record<string, string> = useParams()

	const onSubmit: OnSubmitValidator = async () => {
		const response = await resetPassword({
			userId,
			verificationCode,
			newPassword: newPassword.value,
			confirmNewPassword: confirmNewPassword.value
		})

		if (!response.wasSuccessful) {
			newPassword.setErrors(response.errors[ResetPasswordModelErrorKeys.NewPassword])
			confirmNewPassword.setErrors(response.errors[ResetPasswordModelErrorKeys.ConfirmNewPassword])
			setErrors(response.errors["Unknown"])
		}

		setWasSuccessful(response.wasSuccessful)

		return true
	}

	document.title = getDocumentTitle("Change your password")

	return (
		<>
			{!wasSuccessful && (
				<Form
					handleSubmit={onSubmit}
					validators={[newPassword.validator, confirmNewPassword.validator]}
					title="Reset password"
					formClasses="form--standalone"
					showReset={false}
					showRecaptcha={false}
					showErrors={true}
					errors={errors}
				>
					<TextField
						labelText="New password"
						hint="Your password should be a mix of numbers, symbols, uppercase and lowercase letters at least 8 characters long"
						type="password"
						required={true}
						value={newPassword.value}
						setValue={newPassword.setValue}
						errors={newPassword.errors}
						validator={newPassword.validator}
					/>
					<TextField
						labelText="Confirm new password"
						type="password"
						required={true}
						value={confirmNewPassword.value}
						setValue={confirmNewPassword.setValue}
						errors={confirmNewPassword.errors}
						validator={confirmNewPassword.validator}
					/>
				</Form>
			)}
			{wasSuccessful && (
				<p className="app__message-container">
					Your password was reset successfully. You can now <Link to="/users/login">log in</Link> using your new password.
				</p>
			)}
		</>
	)
}

export default ResetPassword