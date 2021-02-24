import * as React from "react"
import { Dispatch, SetStateAction, useState } from "react"

import Form from "@/components/ui/forms/Form"
import { OnSubmitValidator } from "@/tools/definitions/forms"
import { useFormInputValidation } from "@/hooks/forms"
import { required, requireDigit, requireLength, requireLowerCase, requireMatch, requireSpecialChar, requireUpperCase } from "@/tools/validations"
import TextField from "@/components/ui/forms/fields/TextField"
import { initiatePasswordChange } from "@/tools/browser/users"
import { InitiatePasswordChangeModelErrorKeys } from "@/tools/definitions/users"
import { getDocumentTitle } from "@/tools/utils"

const AccountPassword: React.FunctionComponent = (): JSX.Element => {

	const [wasSuccessful, setWasSuccessful] = useState(false)
	const [errors, setErrors]: [string[], Dispatch<SetStateAction<string[]>>] = useState([])

	const newPassword = useFormInputValidation<string>({
		initialValue: "",
		validators: [
			required,
			requireUpperCase,
			requireLowerCase,
			requireDigit,
			requireSpecialChar,
			(pw: string) => requireLength(pw, 8, 64)
		]
	})
	const confirmNewPassword = useFormInputValidation<string>({
		initialValue: ""
	})
	const currentPassword = useFormInputValidation<string>({
		initialValue: "",
		validators: [required]
	})

	confirmNewPassword.validators.push(() => requireMatch(newPassword.value, confirmNewPassword.value, "The passwords do not match"))

	const onSubmit: OnSubmitValidator = async () => {
		const response = await initiatePasswordChange({
			newPassword: newPassword.value,
			confirmNewPassword: confirmNewPassword.value,
			currentPassword: currentPassword.value
		})

		if (!response.wasSuccessful) {
			newPassword.setErrors(response.errors[InitiatePasswordChangeModelErrorKeys.NewPassword])
			confirmNewPassword.setErrors(response.errors[InitiatePasswordChangeModelErrorKeys.ConfirmNewPassword])
			currentPassword.setErrors(response.errors[InitiatePasswordChangeModelErrorKeys.CurrentPassword])
			setErrors(response.errors["Unknown"])
		}

		setWasSuccessful(response.wasSuccessful)

		return response.wasSuccessful
	}

	document.title = getDocumentTitle("Update your password")

	return (
		<>
			{!wasSuccessful && (
				<Form
					validators={[]}
					handleSubmit={onSubmit}
					title="Account password"
					showReset={false}
					showRecaptcha={false}
					showErrors={true}
					errors={errors}
				>
					<TextField
						labelText="New password"
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
					<TextField
						labelText="Confirm your password"
						type="password"
						required={true}
						value={currentPassword.value}
						setValue={currentPassword.setValue}
						errors={currentPassword.errors}
						validator={currentPassword.validator}
					/>
				</Form>
			)}
			{wasSuccessful && (
				<p>
					Your password has been updated successfully. You will need to use your new password the next time you log in.
				</p>
			)}
		</>
	)
}

export default AccountPassword