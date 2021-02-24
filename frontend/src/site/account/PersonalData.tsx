import * as React from "react"
import { Dispatch, SetStateAction, useState } from "react"
import { useDispatch } from "react-redux"
import { useHistory } from "react-router-dom"

import Form from "@/components/ui/forms/Form"
import Modal from "@/components/ui/Modal"
import TextField from "@/components/ui/forms/fields/TextField"
import { useFormInputValidation } from "@/hooks/forms"
import { required } from "@/tools/validations"
import { deleteAccount } from "@/tools/browser/users"
import { logout } from "@/site/currentUser"
import FormErrorList from "@/components/ui/forms/FormErrorList"
import { getDocumentTitle } from "@/tools/utils"

const PersonalData: React.FunctionComponent = (): JSX.Element => {

	const dispatch = useDispatch()
	const history = useHistory()

	const confirmPassword = useFormInputValidation<string>({
		initialValue: "",
		validators: [required]
	})

	const [showModal, setShowModal] = useState(false)
	const [errors, setErrors]: [string[], Dispatch<SetStateAction<string[]>>] = useState([])

	const handleDeleteDataClick = () => {
		setShowModal(true)
	}

	const deleteData = async () => {
		const response = await deleteAccount({
			confirmPassword: confirmPassword.value
		})

		if (response.wasSuccessful) {
			dispatch(logout())
			setShowModal(false)
			history.push("/")
		} else {
			confirmPassword.setErrors(response.errors["ConfirmPassword"])
			setErrors(response.errors["Unknown"])
		}
	}

	document.title = getDocumentTitle("Get your personal data")

	return (
		<section>
			<Form
				handleSubmit={() => false}
				title="Personal Data"
				showReset={false}
				showSubmit={false}
				showRecaptcha={false}
				showLegend={false}
			>
				<p>
					You have the right to know what data we possess about you. You also have the right to remove your data from our servers.
				</p>
				<p>
					If you choose to remove your data from our servers, you will lose your account and all your settings.
				</p>

				<div className="form__control-wrapper">
					<a
						className="form__control"
						type="button"
						href="/api/account/data"
						target="_blank"
					>
						Download Personal Data
					</a>
					<button
						className="form__control form__control--danger"
						type="button"
						onClick={handleDeleteDataClick}
					>
						Delete Account
					</button>
				</div>

				{showModal && (
					<Modal
						setShowModal={setShowModal}
						modalContentClasses="app__widget app__widget--sm"
						confirmAction={deleteData}
						confirmActionMessage="Delete my account"
						confirmButtonClasses="modal__control--warning"
						cancelActionMessage="No, keep my account"
						cancelButtonClasses="modal__control--warning"
					>
						<p>
							Are you sure you want to delete your account? This can&apos;t be undone.
						</p>
						<TextField
							labelText="Confirm password"
							type="password"
							required={true}
							value={confirmPassword.value}
							setValue={confirmPassword.setValue}
							errors={confirmPassword.errors}
						/>
						{errors.length > 0 && (
							<FormErrorList errors={errors}/>
						)}
					</Modal>
				)}
			</Form>
		</section>
	)
}

export default PersonalData