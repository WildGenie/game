import * as React from "react"
import { NavLink } from "react-router-dom"
import { getDocumentTitle } from "@/tools/utils"

const EditSuccessful: React.FunctionComponent = (): JSX.Element => {

	document.title = getDocumentTitle("Edit successful")

	return (
		<p>
			Species was edited successfully! <NavLink to="/admin/species">Return to species list</NavLink>
		</p>
	)
}

export default EditSuccessful