import * as React from "react"
import { Dispatch, SetStateAction } from "react"

import "./Modal.scss"

type ModalTypes = {
	setShowModal: Dispatch<SetStateAction<boolean>>,
	showConfirmAction?: boolean,
	confirmActionMessage?: string,
	confirmAction?: VoidFunction,
	showCancelAction?: boolean,
	cancelActionMessage?: string,
	cancelAction?: VoidFunction,
	children: React.ReactNode,
	modalClasses?: string,
	modalContentClasses?: string,
	confirmButtonClasses?: string,
	cancelButtonClasses?: string
}

const Modal: React.FunctionComponent<ModalTypes> = ({
	setShowModal,
	showConfirmAction = true,
	confirmActionMessage = "Confirm",
	confirmAction,
	showCancelAction = true,
	cancelActionMessage = "Cancel",
	cancelAction,
	children,
	modalClasses = "",
	modalContentClasses = "",
	confirmButtonClasses = "",
	cancelButtonClasses = ""
}: ModalTypes): JSX.Element => {

	const handleConfirmClick = () => {
		if (confirmAction)
			confirmAction()
	}

	const handleCancelClick = () => {
		if (cancelAction)
			cancelAction()

		setShowModal(false)
	}

	return (
		<article className={`modal ${modalClasses}`}>
			<div
				className="modal__overlay"
				onClick={handleCancelClick}
			/>
			<div className={`modal__content ${modalContentClasses}`}>
				{children}
				<div className="modal__control-wrapper">
					{showConfirmAction && (
						<button
							className={`modal__control modal__control--confirm ${confirmButtonClasses}`}
							onClick={handleConfirmClick}
						>
							{confirmActionMessage}
						</button>
					)}
					{showCancelAction && (
						<button
							className={`modal__control modal__control--cancel ${cancelButtonClasses}`}
							onClick={handleCancelClick}
						>
							{cancelActionMessage}
						</button>
					)}
				</div>
			</div>
		</article>
	)
}

export default Modal