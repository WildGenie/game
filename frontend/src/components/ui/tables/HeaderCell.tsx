import * as React from "react"
import { PropsWithChildren } from "react"

type HeaderCellProps = {
	width?: string
}

const HeaderCell: React.FunctionComponent<PropsWithChildren<HeaderCellProps>> = ({ width, children }: PropsWithChildren<HeaderCellProps>): JSX.Element => {
	return (
		<th
			className="table__header-cell"
			style={{width}}
		>
			{ children }
		</th>
	)
}

export default HeaderCell