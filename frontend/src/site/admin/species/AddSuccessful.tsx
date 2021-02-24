import * as React from "react"
import { NavLink } from "react-router-dom"
import { getDocumentTitle } from "@/tools/utils"

const AddSuccessful: React.FunctionComponent = (): JSX.Element => {

	document.title = getDocumentTitle("Add successful")

	return (
		<p>
			Species was added successfully! <NavLink to="/admin/species/add">Add another</NavLink>
		</p>
	)
}

export default AddSuccessful